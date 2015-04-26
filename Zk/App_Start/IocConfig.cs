using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Zk.Models;
using Zk.Services;
using Zk.Repositories;

namespace Zk
{
    public static class IocConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<ZkContext>()
                .As<IZkContext>()
                .InstancePerRequest();

            builder.RegisterType<Repository>()
                .InstancePerRequest();
            builder.RegisterType<AuthenticationService>()
                .InstancePerRequest();
            builder.RegisterType<CookieProvider>()
                .InstancePerRequest();
            builder.RegisterType<UserService>()
                .InstancePerRequest();

            builder.RegisterType<AnonymousUserService>()
                .Keyed<IUserService>(AuthenticatedStatus.Anonymous);
            builder.RegisterType<UserService>()
                .Keyed<IUserService>(AuthenticatedStatus.Authenticated);

            builder.RegisterType<PasswordRecoveryService>()
                .InstancePerRequest();
            builder.RegisterType<CalendarService>()
                .InstancePerRequest();
            builder.RegisterType<FarmingActionService>()
                .InstancePerRequest();
            builder.RegisterType<CropProvider>()
                .InstancePerRequest();;

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}