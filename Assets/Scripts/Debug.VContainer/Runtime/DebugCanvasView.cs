using System;
using UnityEngine;

namespace BashoKit.Container.Debug
{
    public class DebugCanvasView : MonoBehaviour {
        private Canvas canvas;
        public Transform PanelContainer;
        public Transform TabContainer;

        private bool _isDebugVisible;
        private bool isDebugVisible {
            get => _isDebugVisible;
            set {
                _isDebugVisible = value;
                canvas.enabled = _isDebugVisible;
            }
        }

        private void Awake() {
            canvas = GetComponent<Canvas>();
            isDebugVisible = false;
        }

        private void Update() {
            if (Input.GetKeyUp(KeyCode.F1)) {
                isDebugVisible = !isDebugVisible;
            } else if (isDebugVisible && Input.GetKeyUp(KeyCode.Escape)) {
                isDebugVisible = false;
            }
        }
    }
}