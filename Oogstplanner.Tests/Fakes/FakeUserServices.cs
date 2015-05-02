﻿using Autofac.Features.Indexed;

using Oogstplanner.Repositories;
using Oogstplanner.Services;

namespace Oogstplanner.Tests
{
    public class FakeUserServices : IIndex<AuthenticatedStatus, IUserService>
    {
        readonly Repository repository;

        public FakeUserServices(Repository repository)
        {
            this.repository = repository;
        }

        public bool TryGetValue(AuthenticatedStatus key, out IUserService value)
        {
            value = key == AuthenticatedStatus.Authenticated 
                ? new UserService(repository, new CookieProvider()) as IUserService 
                : new AnonymousUserService(repository, new CookieProvider());
            return true;
        }

        public IUserService this[AuthenticatedStatus index]
        {
            get
            {
                return index == AuthenticatedStatus.Authenticated 
                    ? new UserService(repository, new CookieProvider()) as IUserService
                    : new AnonymousUserService(repository, new CookieProvider());
            }
        }

    }
}

