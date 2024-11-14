using System.Threading;
using Cysharp.Threading.Tasks;
using PlayVibe;
using Units.Extensions;
using Zenject;

namespace Gameplay.Figure
{
    public class FigureGraphicsFactory : PlaceholderFactory<UniTask<FigureGraphicView>>
    {
        private readonly FigureColorBank colorBank;
        private readonly FigureGraphicsBank graphicsBank;
        private readonly ObjectPoolService objectPoolService;

        public FigureGraphicsFactory(FigureColorBank colorBank, FigureGraphicsBank graphicsBank, ObjectPoolService objectPoolService)
        {
            this.colorBank = colorBank;
            this.graphicsBank = graphicsBank;
            this.objectPoolService = objectPoolService;
        }

        public async UniTask<FigureGraphicView> CreateGraphicView(FigureView figureView, FigureColorType colorType, CancellationToken cancellationToken)
        {
            var graphicAsset = graphicsBank.GetAssetReference(EnumExtensions.GetRandomValue<FigureGraphicType>());
            var graphicView = await objectPoolService.GetOrCreateView<FigureGraphicView>(graphicAsset, figureView.FigureTransform);

            if (cancellationToken.IsCancellationRequested)
            {
                objectPoolService.ReturnToPool(graphicView);
                return null;
            }

            graphicView.ApplyMaterial(colorBank.GetMaterial(colorType), colorType);
            
            return graphicView;
        }
    }
}