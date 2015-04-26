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

            builder.RegisterType<Repository>();
            builder.RegisterType<AuthenticationService>();
            builder.RegisterType<UserService>()
                .Keyed<IUserService>(AuthenticatedStatusEnum.Anonymous);
            builder.RegisterType<UserService>()
                .Keyed<IUserService>(AuthenticatedStatusEnum.Authenticated);
            builder.RegisterType<PasswordRecoveryService>()
                .As<IPasswordRecoveryService>();
            builder.RegisterType<CalendarService>();
            builder.RegisterType<FarmingActionService>();
            builder.RegisterType<CropProvider>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}