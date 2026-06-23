using System;
using System.Collections.Generic;

namespace SentenceStock.Api
{
    public sealed class SimpleServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        public void Add<T>(T service)
            where T : class
        {
            services[typeof(T)] = service;
        }

        public object GetService(Type serviceType)
        {
            object service;
            return services.TryGetValue(serviceType, out service) ? service : null;
        }
    }
}
