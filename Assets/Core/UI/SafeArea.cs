using UnityEngine;

namespace Core.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        [SerializeField] private bool _conformX = true;
        [SerializeField] private bool _conformY = true;
        
        private RectTransform _panel;
        private Rect _lastSafeArea;
        private Vector2Int _lastScreenSize;
        private ScreenOrientation _lastOrientation = ScreenOrientation.AutoRotation;

        private void Awake()
        {
            _panel = GetComponent<RectTransform>();
            Refresh();
        }

        private void Update()
        {
            Refresh();
        }

        private void Refresh()
        {
            var safeArea = Screen.safeArea;

            if (safeArea != _lastSafeArea
                || Screen.width != _lastScreenSize.x
                || Screen.height != _lastScreenSize.y
                || Screen.orientation != _lastOrientation)
            {
                // Fix for having auto-rotate off and manually forcing a screen orientation.
                // See https://forum.unity.com/threads/569236/#post-4473253 and https://forum.unity.com/threads/569236/page-2#post-5166467
                _lastScreenSize.x = Screen.width;
                _lastScreenSize.y = Screen.height;
                _lastOrientation = Screen.orientation;

                ApplySafeArea (safeArea);
            }
        }

        private void ApplySafeArea(Rect rect)
        {
            _lastSafeArea = rect;

            if (!_conformX)
            {
                rect.x = 0;
                rect.width = Screen.width;
            }

            if (!_conformY)
            {
                rect.y = 0;
                rect.height = Screen.height;
            }

            Vector2 anchorMin = rect.position;
            Vector2 anchorMax = rect.position + rect.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            _panel.anchorMin = anchorMin;
            _panel.anchorMax = anchorMax;
        }
    }
}
