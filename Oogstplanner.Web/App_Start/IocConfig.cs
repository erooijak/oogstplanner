using System;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;

using Oogstplanner.Models;
using Oogstplanner.Services;
using Oogstplanner.Repositories;

namespace Oogstplanner
{
    public static class IocConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<OogstplannerContext>()
                .As<IOogstplannerContext>()
                .InstancePerRequest();
                
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<AuthenticationService>()
                .As<IAuthenticationService>()
                .InstancePerRequest();
            builder.RegisterType<CookieProvider>()
                .As<ICookieProvider>()
                .InstancePerRequest();
            builder.RegisterType<MembershipService>()
                .As<IMembershipService>()
                .InstancePerRequest();
            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerRequest();

            builder.RegisterType<AnonymousUserService>()
                .Keyed<IUserService>(AuthenticatedStatus.Anonymous);
            builder.RegisterType<UserService>()
                .Keyed<IUserService>(AuthenticatedStatus.Authenticated);

            builder.RegisterType<PasswordRecoveryService>()
                .As<IPasswordRecoveryService>()
                .InstancePerRequest();
            builder.RegisterType<CalendarService>()
                .As<ICalendarService>()
                .InstancePerRequest();
            builder.RegisterType<FarmingActionService>()
                .As<IFarmingActionService>()
                .InstancePerRequest();
            builder.RegisterType<CropProvider>()
                .As<ICropProvider>()
                .InstancePerRequest();;

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
