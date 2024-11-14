using PlayVibe;
using UnityEngine;

namespace Gameplay.Figure
{
    public class FigureGraphicView : PoolView
    {
        [SerializeField] private MeshRenderer meshRenderer;
        
        public FigureColorType ColorType { get; private set; }

        public void ApplyMaterial(Material material, FigureColorType colorType)
        {
            meshRenderer.material = material;
            ColorType = colorType;
        }
    }
}