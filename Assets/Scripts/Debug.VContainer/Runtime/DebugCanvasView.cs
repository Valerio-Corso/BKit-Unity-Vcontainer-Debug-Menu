using UnityEngine;

namespace BashoKit.Container.Debug
{
    public class DebugCanvasView : MonoBehaviour {
        [SerializeField] Transform _panelContainer;
        [SerializeField] Transform _tabContainer;
        
        private Canvas canvas;
        
        public Transform PanelContainer => _panelContainer;
        public Transform TabContainer => _tabContainer;

        public void ToggleDebugCanvas() {
            canvas.enabled = !canvas.enabled;
        }

        public bool IsVisible => canvas.enabled;
        
        public void CloseDebugCanvas() {
            canvas.enabled = false;
        }

        private void Awake() {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
        }
    }
}