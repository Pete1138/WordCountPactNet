using Microsoft.Practices.Unity;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Fabric;
using System.Web.Http;
using Unity.WebApi;
using WordCount.WebService.Controllers;

namespace WordCount.WebService
{
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration config, ICodePackageActivationContext activationContext)
        {
            UnityContainer container = new UnityContainer();
            container.RegisterType<IServiceUriResolver, ServiceUriResolver>(
                new TransientLifetimeManager(),
                new InjectionConstructor(activationContext));

            container.RegisterType<IServicePartitionResolver, ServicePartitionResolver>(new TransientLifetimeManager(),
                new InjectionConstructor(
                        new CreateFabricClientDelegate(() => new FabricClient())
                    )
                );

            container.RegisterType<IFabricClientQueryManager, FabricClientQueryManager>();
            container.RegisterType<ICommunicationClientFactory<HttpCommunicationClient>, HttpCommunicationClientFactory>();
                //new HttpCommunicationClientFactory(servicePartitionResolver)
            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}