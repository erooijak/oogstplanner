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

            var repositoryFactories = new RepositoryFactories();
            builder.RegisterInstance(repositoryFactories)
                .As<RepositoryFactories>()
                .SingleInstance();
                
            builder.RegisterType<RepositoryProvider>()
                .As<IRepositoryProvider>()
                .InstancePerRequest();

            // Note: If a constructor requests an IUserService this should be of
            //       type UserService and not AnonymousUserService (therefore the Except).
            builder.RegisterAssemblyTypes(typeof(ServiceBase).Assembly)
                .Except<AnonymousUserService>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<AnonymousUserService>()
                .Keyed<IUserService>(AuthenticatedStatus.Anonymous)
                .InstancePerRequest();
            builder.RegisterType<UserService>()
                .Keyed<IUserService>(AuthenticatedStatus.Authenticated)
                .InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
