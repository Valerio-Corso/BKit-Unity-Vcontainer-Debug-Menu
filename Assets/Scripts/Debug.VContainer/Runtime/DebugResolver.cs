using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VContainer;

namespace BashoKit.Container.Debug {
    public static class DebugResolver {
        private static Assembly _assembly;

        public static void SetAssembly(string assemblyName) {
            _assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains(assemblyName));
        }
        
        public static IEnumerable<(MethodInfo method, DebugActionAttribute actionAttribute)> GetDebugActions() {
            foreach (var type in _assembly.GetTypes()) {
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
                    var actionAttr = method.GetCustomAttribute<DebugActionAttribute>();
                    if (actionAttr != null) {
                        yield return (method, actionAttr);
                    }
                }
            }
        }
        
        public static void RegisterAllDebugProviders(IContainerBuilder builder)
        {
            var debugProviderTypes = _assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(IDebugProvider).IsAssignableFrom(t));
            foreach (var type in debugProviderTypes) {
                builder.Register(type, Lifetime.Singleton);
            }
        }
    }
}