using System.Reflection;
using System.Web.Http;
using ASB.Services;
using Autofac;
using Autofac.Integration.WebApi;

namespace ASB.WEB
{
    public static class AutofacConfigure
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterWebApiModelBinderProvider();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<SaveSomethingService>().As<ISaveSomethingService>().InstancePerLifetimeScope();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}