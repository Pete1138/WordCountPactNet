//using FakeItEasy;
//using Microsoft.Practices.Unity;
//using Microsoft.ServiceFabric.Data;
//using System.Web.Http;
//using Unity.WebApi;
//using WordCountService.Controllers;

//namespace WordCount.Service.Tests
//{
//    public static class UnityConfig
//    {
//        public static void RegisterComponents(HttpConfiguration config)
//        {
//            UnityContainer container = new UnityContainer();

//            var stateManager = A.Fake<IReliableStateManager>();

//            container.RegisterType<DefaultController>(
//                new TransientLifetimeManager(),
//                new InjectionConstructor(stateManager));

//            config.DependencyResolver = new UnityDependencyResolver(container);
//        }
//    }
//}