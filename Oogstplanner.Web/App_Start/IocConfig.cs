using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;

using Oogstplanner.Models;
using Oogstplanner.Data;
using Oogstplanner.Services;

namespace Oogstplanner.Web
{
    public static class IocConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<OogstplannerUnitOfWork>()
                .As<IOogstplannerUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OogstplannerContext>()
                .As<IOogstplannerContext>()
                .InstancePerRequest();

            var repositoryInstances = new RepositoryFactories();
            builder.RegisterInstance(repositoryInstances)
                .As<RepositoryFactories>()
                .SingleInstance();
                
            builder.RegisterType<RepositoryProvider>()
                .As<IRepositoryProvider>()
                .InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(ServiceBase).Assembly)
                .AsImplementedInterfaces()
                .Except<UserService>()
                .Except<AnonymousUserService>();

            builder.RegisterType<AnonymousUserService>()
                .Keyed<IUserService>(AuthenticatedStatus.Anonymous);
            builder.RegisterType<UserService>()
                .Keyed<IUserService>(AuthenticatedStatus.Authenticated);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
