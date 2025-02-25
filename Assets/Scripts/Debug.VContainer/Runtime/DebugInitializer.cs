using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BashoKit.Container.Debug;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace BashoKit.Debug.Runtime {
    public class DebugInitializer : ITickable {
        private readonly DebugInstanceRegistry _registry;
        private readonly DebugCanvasReferences _debugReferences;
        
        private readonly Dictionary<string, Button> _tabButtons = new Dictionary<string, Button>();
        private readonly Dictionary<string, GameObject> _tabPanels = new Dictionary<string, GameObject>();
        private DebugCanvasView _canvasView;
        
        private bool IsInitialized => _canvasView != null;

        public DebugInitializer(DebugInstanceRegistry registry, DebugCanvasReferences debugReferences) {
            _registry = registry;
            _debugReferences = debugReferences;
        }
        
        public void Tick() {
            if (Input.GetKeyUp(KeyCode.F1)) {
                if (!IsInitialized) Initialize();
                _canvasView?.ToggleDebugCanvas();
            } else if (_canvasView is { IsVisible: true } && Input.GetKeyUp(KeyCode.Escape)) {
                _canvasView?.CloseDebugCanvas();
            }
        }

        public void Initialize() {
            var canvasGo = Object.Instantiate(_debugReferences.CanvasPrefab);
            _canvasView = canvasGo.GetComponent<DebugCanvasView>();
            
            var debugMethods = DebugResolver.GetDebugActions();

            // Group by header, determined from the declaring type.
            var groupedMethods = debugMethods.GroupBy(tuple => {
                var headerAttr = tuple.method.DeclaringType.GetCustomAttribute<DebugTabAttribute>();
                return headerAttr != null ? headerAttr.TabName : "N/A";
            });

            foreach (var debugGroup in groupedMethods) {
                // Instantiate a panel for each group
                var panel = Object.Instantiate(_debugReferences.cheatPanelContainerPrefab, _canvasView.PanelContainer);
                var panelContentTransform = panel.transform;
                CreateTab(_canvasView, debugGroup.Key, panel);

                // Create buttons for each method with header
                IEnumerable<IGrouping<string,(MethodInfo method, DebugActionAttribute actionAttribute)>> headersGrouping = debugGroup.GroupBy(p => p.actionAttribute.HeaderName).ToList();
                foreach (var headerGroup in headersGrouping) {
                    CreateHeader(headerGroup, panelContentTransform);
                    foreach (var (method, attribute) in headerGroup) {
                        CreateDebugActions(panelContentTransform, attribute, method);
                    }
                }
            }

            SelectFirstTab(groupedMethods);
        }

        private void SelectFirstTab(IEnumerable<IGrouping<string, (MethodInfo method, DebugActionAttribute actionAttribute)>> groupedMethods)
        {
            if (!groupedMethods.Any()) return;
            
            var firstTab = groupedMethods.First();
            _tabButtons[firstTab.Key].Select();
            ChangeTab(firstTab.Key);
        }

        private void CreateDebugActions(Transform panelContentTransform, DebugActionAttribute attribute, MethodInfo method)
        {
            var buttonObj = Object.Instantiate(_debugReferences.CheatButtonPrefab, panelContentTransform);
            var btn = buttonObj.GetComponent<Button>();
            var btnText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = string.IsNullOrEmpty(attribute.DisplayName) ? method.Name : attribute.DisplayName;;

            btn.onClick.AddListener(() => {
                object instance = _registry.GetInstance(method.DeclaringType);
                if (method.IsStatic) {
                    method.Invoke(null, null);
                }
                else {
                    // Look up the instance from the registry.
                    if (instance != null) {
                        method.Invoke(instance, null);
                    }
                }
            });
        }

        private void CreateHeader(IGrouping<string, (MethodInfo method, DebugActionAttribute actionAttribute)> unused, Transform panelContentTransform)
        {
            var headerName = unused.Key;
            if (headerName == null) return;
            
            var header = Object.Instantiate(_debugReferences.cheatHeaderPrefab, panelContentTransform);
            header.GetComponent<TextMeshProUGUI>().text = headerName;
        }

        private void CreateTab(DebugCanvasView canvasView, string tabName, GameObject panel)
        {
            var tab = Object.Instantiate(_debugReferences.tabPrefab, canvasView.TabContainer);
            var tabBtn = tab.GetComponent<Button>();
            var tabText = tab.GetComponentInChildren<TextMeshProUGUI>();
            tabText.text = tabName;
                
            tabBtn.onClick.AddListener(() => {
                ChangeTab(tabBtn.GetComponentInChildren<TextMeshProUGUI>().text);
            });
                
            _tabButtons.Add(tabName, tabBtn);
            _tabPanels.Add(tabName, panel);
        }
        public void ChangeTab(string tabName) {
            foreach (var (name, tab) in _tabPanels) {
                tab.SetActive(name.Equals(tabName));
            }
        }
    }
}