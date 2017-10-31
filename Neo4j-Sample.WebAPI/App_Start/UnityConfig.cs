using Neo4j_Sample.Helpers;
using Neo4j_Sample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Exceptions;
using Unity.Injection;

namespace Neo4j_Sample.WebAPI
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            config.DependencyResolver = new UnityResolver(container);

            //TODO: get from config file
            var connection = new Connection
            {
                Url = "bolt://127.0.0.1:7687/",
                UserName = "neo4j",
                Password = "asdf"
            };

            container.RegisterType<Connection>(new InjectionFactory(c => connection));
            container.RegisterType<PersonRepository>();

            //for a service which exposes its interface
            //container.RegisterType<IProductRepository, ProductRepository>(new HierarchicalLifetimeManager());
        }
    }

    internal class UnityResolver : IDependencyResolver
    {
        protected IUnityContainer container;

        public UnityResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new UnityResolver(child);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            container.Dispose();
        }
    }
}