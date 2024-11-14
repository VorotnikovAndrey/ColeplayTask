using System;
using DG.Tweening;
using PlayVibe;
using UniRx;
using UnityEngine;

namespace Gameplay.Figure
{
    public class FigureView : PoolView
    {
        [SerializeField] private Transform figureTransform;
        
        private readonly Subject<FigureView> clickSubject = new();
        
        public Transform FigureTransform => figureTransform;
        
        public IObservable<FigureView> OnClick => clickSubject;
        public FigureGraphicView GraphicView { get; private set; }

        private void OnDestroy()
        {
            if (DOTween.IsTweening(figureTransform))
            {
                DOTween.Kill(figureTransform);
            }
        }
        
        public override void OnReturnToPool()
        {
            base.OnReturnToPool();
            
            DropGraphic();
        }
        
        public void SnapGraphic(FigureGraphicView graphicView)
        {
            if (GraphicView != null)
            {
                DropGraphic();
            }
            
            GraphicView = graphicView;
        }

        public void DropGraphic()
        {
            if (GraphicView == null)
            {
                return;
            }
            
            objectPoolService.ReturnToPool(GraphicView);
            GraphicView = null;
        }

        public void HandleClick()
        {
            if (GraphicView == null)
            {
                return;
            }

            if (clickSubject.HasObservers)
            {
                clickSubject.OnNext(this);
            }
        }
        
        public void PulsateEffect()
        {
            figureTransform.DOScale(1.1f, 0.2f).OnKill(() => figureTransform.DOScale(1f, 0.2f));
        }
    }
}