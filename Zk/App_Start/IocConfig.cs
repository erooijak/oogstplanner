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
            builder.RegisterType<UserService>();
            builder.RegisterType<CalendarService>();
            builder.RegisterType<FarmingActionService>();
            builder.RegisterType<CropProvider>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}