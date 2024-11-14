using Cysharp.Threading.Tasks;
using Gameplay.Events;
using Gameplay.Figure;
using PlayVibe;
using TMPro;
using UnityEngine;
using Zenject;

namespace Popups.HudPopup
{
    public class HudPopup : AbstractBasePopup
    {
        [SerializeField] private TextMeshProUGUI titleText;

        [Inject] private FigureColorBank figureColorBank;
        
        protected override UniTask OnShow(object data = null)
        {
            if (data is FigureColorType targetColor)
            {
                UpdateText(targetColor);
            }
            else
            {
                Hide(true).Forget();
            }
            
            Subscribes();
            
            return UniTask.CompletedTask;
        }

        protected override UniTask OnHide()
        {
            Unsubscribes();
            
            return UniTask.CompletedTask;
        }

        protected override void OnShowen()
        {
            
        }

        protected override void OnHiden()
        {
            
        }
        private void Subscribes()
        {
            eventAggregator.Add<NextRoundEvent>(OnNextRoundEvent);
        }

        private void Unsubscribes()
        {
            eventAggregator.Remove<NextRoundEvent>(OnNextRoundEvent);
        }

        private void OnNextRoundEvent(NextRoundEvent eventData) => UpdateText(eventData.TargetColor);

        private void UpdateText(FigureColorType colorType)
        {
            titleText.text = string.Format(Constants.Text.TargetColorFormat, colorType, figureColorBank.GetMaterial(colorType).color.ToHtmlStringRGB());
        }
    }
}