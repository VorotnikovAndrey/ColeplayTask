using Gameplay.Figure;
using PlayVibe;
using UniRx;
using UnityEngine;

namespace Gameplay
{
    public class RaycastChecker : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private LayerMask layerMask;

        private const float rayDistance = 100f;

        public DisableHandler DisableHandler { get; } = new();
        
        private void Start()
        {
            Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0)).Subscribe(_ => CastRay()).AddTo(this);
        }
        
        private void CastRay()
        {
            if (DisableHandler.IsDisabled)
            {
                return;
            }
            
            var ray = cam.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, rayDistance, layerMask))
            {
                return;
            }

            var view = hit.collider.GetComponent<FigureView>();
            
            if (view != null)
            {
                view.HandleClick();
            }
        }
    }
}