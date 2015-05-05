using Autofac.Features.Indexed;

using Oogstplanner.Repositories;
using Oogstplanner.Services;

namespace Oogstplanner.Tests
{
    public class FakeUserServices : IIndex<AuthenticatedStatus, IUserService>
    {
        readonly UserRepository userRepository;
        readonly CalendarRepository calendarRepository;

        public FakeUserServices(UserRepository userRepository, CalendarRepository calendarRepository)
        {
            this.userRepository = userRepository;
            this.calendarRepository = calendarRepository;
        }

        public bool TryGetValue(AuthenticatedStatus key, out IUserService value)
        {
            value = key == AuthenticatedStatus.Authenticated 
                ? new UserService(userRepository, calendarRepository, new CookieProvider()) as IUserService 
                : new AnonymousUserService(userRepository, calendarRepository, new CookieProvider());
            return true;
        }

        public IUserService this[AuthenticatedStatus index]
        {
            get
            {
                return index == AuthenticatedStatus.Authenticated 
                    ? new UserService(userRepository, calendarRepository, new CookieProvider()) as IUserService
                        : new AnonymousUserService(userRepository, calendarRepository, new CookieProvider());
            }
        }

    }
}

