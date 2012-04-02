using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace nKanban.Infrastructure.IoC
{
    public class SmDependencyResolver : IDependencyResolver
    {
        private readonly IContainer _container;

        public SmDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            var service = _container.TryGetInstance(serviceType);

            if (service == null && !serviceType.IsAbstract)
            {
                _container.Configure(c => c.AddType(serviceType, serviceType));
                service = _container.TryGetInstance(serviceType);
            }

            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType).Cast<object>();
        }
    }
}