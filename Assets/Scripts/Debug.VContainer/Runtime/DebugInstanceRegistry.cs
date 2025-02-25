using System;
using System.Collections.Generic;
using VContainer;

namespace BashoKit.Container.Debug
{
    public class DebugInstanceRegistry {
        private readonly IObjectResolver _resolver;
        private Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        public DebugInstanceRegistry(IObjectResolver resolver) {
            _resolver = resolver;
        }

        public object GetInstance(Type type) {
            // _instances.TryGetValue(type, out var instance);
            return _resolver.Resolve(type);
        }
    }
}