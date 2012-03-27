using System.Web.Mvc;
using StructureMap;
using System.Web;
using System.Configuration;
using System;
using MongoDB.Driver;
using nKanban.Persistence;
using nKanban.Persistence.MongoDb;
using nKanban.Shared;

namespace nKanban.Infrastructure.IoC
{
    public static class IoCFactory
    {
        public static void CommitAndDisposeHttpContextScopedObjects()
        {
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }

        public static IDependencyResolver CreateDependencyResolver()
        {
            Bootstrap();
            return new SmDependencyResolver(ObjectFactory.Container);
        }

        public static T GetService<T>()
        {
            return ObjectFactory.GetInstance<T>();
        }

        private static void Bootstrap()
        {
            ObjectFactory.Configure(c =>
            {
                Func<MongoDatabase> openDbFunc = () =>
                {
                    var mongo = MongoServer.Create(ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString);
                    return mongo.GetDatabase(ConfigurationManager.AppSettings["MongoDatabaseName"]);
                };

                c.For<MongoDatabase>().HttpContextScoped().Use(openDbFunc);

                c.Scan(scanner => {
                    scanner.TheCallingAssembly();
                    //grab all ColorMatchR assemblies from the base directory
                    scanner.AssembliesFromApplicationBaseDirectory(assembly => { return assembly.FullName.StartsWith("nKanban"); });
                    scanner.WithDefaultConventions();
                });

                c.For<IHttpContext>().Use<LocalHttpContextWrapper>();
                c.For<IRepository>().Use<MongoDbRepository>();
            });
        }
    }
}