using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Zk.Models;
using Zk.BusinessLogic;
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
            builder.RegisterType<UserManager>();
            builder.RegisterType<CalendarManager>();
            builder.RegisterType<FarmingActionManager>();
            builder.RegisterType<CropManager>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}