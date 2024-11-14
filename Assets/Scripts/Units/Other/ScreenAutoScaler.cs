using UnityEngine;
using UnityEngine.UI;

namespace PlayVibe
{
    [RequireComponent(typeof(CanvasScaler))]
    public sealed class ScreenAutoScaler : MonoBehaviour
    {
        [HideInInspector] [SerializeField] private CanvasScaler canvasScaler;

        [SerializeField] private Vector2Int defaultResolution = new(1920, 1080);
        [SerializeField] private bool enable = true;

        private float currentScreenWidth;
        private float currentScreenHeight;
        private Vector2Int screenSize;

       private void OnValidate()
        {
            canvasScaler = GetComponent<CanvasScaler>();
        }

        private void Start()
        {
            if (!enable)
            {
                return;
            }

            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            
            UpdateResolution();
            
            currentScreenWidth = Screen.width;
            currentScreenHeight = Screen.height;
        }

        private void Update()
        {
            if (Screen.width == currentScreenWidth && Screen.height == currentScreenHeight)
            {
                return;
            }
            
            currentScreenWidth = Screen.width;
            currentScreenHeight = Screen.height;
                
            UpdateResolution();
        }

        private void UpdateResolution()
        {
            screenSize = new Vector2Int(Screen.width, Screen.height);
            
            Screen.SetResolution(screenSize.x, screenSize.y, Screen.fullScreenMode);

            var factorX = (float) screenSize.x / (float) defaultResolution.x;
            var factorY = (float) screenSize.y / (float) defaultResolution.y;
            var coef = Mathf.Clamp(factorY - factorX, 0, float.MaxValue);

            canvasScaler.scaleFactor = factorY - coef;
        }
    }
}