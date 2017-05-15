using System.Web.Http;
//using Autofac;
//using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using WordCount.Service.Tests;
using Microsoft.Practices.Unity;
using Unity.WebApi;
using System.Web.Http.Dispatcher;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;
using System.Linq;
using Microsoft.ServiceFabric.Data;
using Autofac;
using WordCountService.Controllers;
using Autofac.Integration.WebApi;
using System.Web.Http.Description;

[assembly: OwinStartup("ApiConfiguration", typeof(Startup))]
namespace WordCount.Service.Tests
{
    public class Startup
    {
        public void Configuration(IAppBuilder app, HttpConfiguration config)
        {
            var tracing = config.EnableSystemDiagnosticsTracing();
            tracing.IsVerbose = true;
            tracing.MinimumLevel = System.Web.Http.Tracing.TraceLevel.Debug;

            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            
            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
            );
            
            //UnityConfig.RegisterComponents(config);

            config.Services.Replace(typeof(IAssembliesResolver), new TestApiAssemblyResolver());

            app.UseWebApi(config);
            //var stateManager = A.Fake
            //var builder = new ContainerBuilder();
            //builder.RegisterType<IReliableStateManager>()
            //    builder.Register(typeof(IReliableStateManager), (c, o) => stateManager);
            //builder.RegisterApiControllers(typeof(DefaultController).Assembly);
            //var container = builder.Build();

            app.UseAutofacWebApi(config);
            
        }

    }

    public class TestApiAssemblyResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            List<Assembly> baseAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WordCount.Service.exe");
            /*
             You can try unreferencing webapiCheck.dll from this project but it makes no difference.
             */
            //var path = @"Your path...Documents\visual studio 2015\Projects\WebApiCheck\WebApiCheck\bin\webapiCheck.dll";

            var controllersAssembly = Assembly.LoadFrom(path);
            baseAssemblies.Add(controllersAssembly);
            return baseAssemblies;
        }

    }
}
