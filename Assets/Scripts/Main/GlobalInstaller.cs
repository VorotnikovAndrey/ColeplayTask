using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Figure;
using Services;
using UnityEngine;
using Zenject;

namespace PlayVibe
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private FigureGraphicsBank figureGraphicsBank;
        [SerializeField] private FigureColorBank figureColorBank;

        public override void InstallBindings()
        {
            Application.targetFrameRate = 60;
            
            BindBase();
            BindScriptableObject();
            BindFactories();
            BindServices();
            BindStages();
        }

        private void BindScriptableObject()
        {
            Container.Bind<FigureGraphicsBank>().FromInstance(figureGraphicsBank).AsSingle().NonLazy();
            Container.Bind<FigureColorBank>().FromInstance(figureColorBank).AsSingle().NonLazy();
        }

        private void BindBase()
        {
            Container.Bind<EventAggregator>().AsSingle().NonLazy();
        }

        private void BindFactories()
        {
            Container.BindFactory<ScreenFaderBase, ScreenFaderFactory>().AsSingle();
            Container.BindFactory<UniTask<FigureGraphicView>, FigureGraphicsFactory>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<ObjectPoolService>().AsSingle().NonLazy();
            Container.Bind<PopupService>().AsSingle().NonLazy();
            Container.Bind<EscService>().AsSingle().NonLazy();
        }

        private void BindStages()
        {
            Container.BindStageService()
                .BindStage<GameplayStage>()
                .SetStageAsync<GameplayStage>().Forget();
        }
    }
}