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

            // Note: ServiceBase     is the base class for both AnonymousUserService and
            //       UserService.
            //       If a constructor requests an IUserService this should be of
            //       type UserService and not AnonymousUserService.
            //       To accomplish this the AnonymousUserService is not registered below.
            //
            //       Furthermore, the LastActivityUpdator and the decorator 
            //       AnonymousUserLastActivityUpdator are registered differently.
            builder.RegisterAssemblyTypes(typeof(ServiceBase).Assembly)
                .Except<UserService>()
                .Except<AnonymousUserService>()
                .Except<LastActivityUpdator>()
                .Except<AnonymousUserLastActivityUpdator>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<LastActivityUpdator>()
                .Named<ILastActivityUpdator>("lastActivityUpdator")
                .InstancePerRequest();
            builder.RegisterType<AnonymousUserLastActivityUpdator>()
                .Named<ILastActivityUpdator>("anonymousUserLastActivityUpdator")
                .InstancePerRequest();

            builder.RegisterDecorator<ILastActivityUpdator>(
                (c, inner) => c.ResolveNamed<ILastActivityUpdator>("anonymousUserLastActivityUpdator", 
                    TypedParameter.From(inner)),
                fromKey: "lastActivityUpdator")
                .As<ILastActivityUpdator>()
                .InstancePerRequest();

            builder.Register(c => new UserService(
                c.Resolve<IOogstplannerUnitOfWork>(),
                c.Resolve<ICookieProvider>(),
                c.ResolveNamed<ILastActivityUpdator>("lastActivityUpdator")))
                .As<IUserService>()
                .InstancePerRequest();
            builder.Register(c => new AnonymousUserService(
                c.Resolve<IOogstplannerUnitOfWork>(),
                c.Resolve<ICookieProvider>(),
                c.ResolveNamed<ILastActivityUpdator>("anonymousUserLastActivityUpdator")))
                .InstancePerRequest();
      
            builder.RegisterType<UserService>()
                .Keyed<IUserService>(AuthenticatedStatus.Authenticated)
                .InstancePerRequest();
            builder.RegisterType<AnonymousUserService>()
                .Keyed<IUserService>(AuthenticatedStatus.Anonymous)
                .WithParameter(
                    (p, c) => p.Name == "updator", 
                    (p, c) => c.ResolveNamed<ILastActivityUpdator>("anonymousUserLastActivityUpdator"))
                .InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
