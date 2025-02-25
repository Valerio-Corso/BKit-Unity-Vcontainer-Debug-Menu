using BashoKit.Debug.Runtime;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BashoKit.Container.Debug {
    public class DebugInstaller : LifetimeScope {
        [SerializeField] private DebugCanvasReferences _canvasReferences;
        [SerializeField] private string _assemblyName;

        protected override void Configure(IContainerBuilder builder) {
            DebugResolver.SetAssembly(_assemblyName);
            builder.RegisterEntryPoint<DebugInitializer>().WithParameter(_canvasReferences);
            builder.Register<DebugInstanceRegistry>(Lifetime.Singleton);
            DebugResolver.RegisterAllDebugProviders(builder);
        }
    }
}