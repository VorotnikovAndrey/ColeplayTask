using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Events;
using Gameplay.Figure;
using PlayVibe;
using Sirenix.Utilities;
using UniRx;
using Units.Extensions;
using UnityEngine;
using Zenject;
using ColorUtility = PlayVibe.ColorUtility;

namespace Gameplay
{
    public sealed class GameplayBootstrap : MonoBehaviour, HColor
    {
        [HideInInspector] [SerializeField] private HColorData hColorData;

        [SerializeField] private RaycastChecker raycastChecker;
        [SerializeField] private List<Transform> points;

        [Inject] private EventAggregator eventAggregator;
        [Inject] private ObjectPoolService objectPoolService;
        [Inject] private FigureGraphicsBank graphicsBank;
        [Inject] private FigureColorBank colorBank;
        [Inject] private PopupService popupService;
        [Inject] private FigureGraphicsFactory graphicsFactory;

        private FigureColorType targetColor;
        
        private readonly Dictionary<int, FigureView> figures = new();
        private readonly CancellationTokenSource cancellationTokenSource = new();
        
        public HColorData HColorData => hColorData;

        private void OnValidate()
        {
            hColorData.TextColor = ColorUtility.HexToColor("#7aaeff");
        }

        private void Start()
        {
            Subscribes();
            Run().Forget();
        }

        private void OnDestroy()
        {
            Unsubscribes();
            
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();

            figures.ForEach(figure => objectPoolService.ReturnToPool(figure.Value));
            figures.Clear();
        }
        
        private void Subscribes()
        {
            eventAggregator.Add<NextRoundEvent>(OnNextRoundEvent);
        }

        private void Unsubscribes()
        {
            eventAggregator.Remove<NextRoundEvent>(OnNextRoundEvent);
        }
        
        private void OnNextRoundEvent(NextRoundEvent eventData) => RefreshFigureGraphics(cancellationTokenSource.Token).Forget();
        
        private async UniTask Run()
        {
            await CreateViews(cancellationTokenSource.Token);
            
            targetColor = EnumExtensions.GetRandomValueExcluding(targetColor);
            popupService.ShowPopup(new PopupOptions(Constants.Popups.HudPopup, targetColor)).Forget();
            eventAggregator.SendEvent(new NextRoundEvent(targetColor));
        }
        
        private async UniTask CreateViews(CancellationToken cancellationToken)
        {
            for (var i = 0; i < points.Count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var view = await objectPoolService.GetOrCreateView<FigureView>(Constants.Views.FigureView, points[i].transform);

                if (cancellationToken.IsCancellationRequested)
                {
                    objectPoolService.ReturnToPool(view);
                    return;
                }
                
                view.OnClick.Subscribe(x =>
                {
                    if (x.GraphicView.ColorType == targetColor)
                    {
                        RoundCompleted();
                    }
                    else
                    {
                        x.PulsateEffect();
                        RoundFailed();
                    }
                }).AddTo(this);

                figures.Add(i, view);
            }
        }

        private async UniTask RefreshFigureGraphics(CancellationToken cancellationToken)
        {
            var figuresList = figures.ToList();
            var availableColors = Enum.GetValues(typeof(FigureColorType)).Cast<FigureColorType>().ToList();
    
            availableColors.Shuffle();
    
            if (availableColors.Count < figuresList.Count)
            {
                Debug.LogError("Not enough materials for all figures.".AddColorTag(Color.red));
                return;
            }

            for (var i = 0; i < figuresList.Count; i++)
            {
                var element = figuresList[i];
                var figureView = element.Value;
                var colorType = availableColors[i];
                var graphicView = await graphicsFactory.CreateGraphicView(figureView, colorType, cancellationToken);
        
                if (graphicView == null)
                {
                    return;
                }
        
                figureView.SnapGraphic(graphicView);
                graphicView.gameObject.SetActive(true);
            }

            figures.ForEach(x => x.Value.gameObject.SetActive(true));
        }

        private void RoundCompleted()
        {
            targetColor = EnumExtensions.GetRandomValueExcluding(targetColor);
            eventAggregator.SendEvent(new NextRoundEvent(targetColor));
        }

        private void RoundFailed()
        {
            popupService.ShowPopup(new PopupOptions(Constants.Popups.InfoPopup, new InfoPopupData(Constants.Text.IncorrectColor))).Forget();
        }
        
        private void OnDrawGizmos()
        {
            foreach (var point in points)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(point.transform.position, 0.2f);
            }
        }
    }
}