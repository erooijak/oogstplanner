using System;

using Autofac.Features.Indexed;
using Zk.Models;
using Zk.Services;
using Zk.Repositories;

namespace Zk.Tests
{
    public class FakeUserServices : IIndex<AuthenticatedStatusEnum, IUserService>
    {
        readonly Repository repository;

        public FakeUserServices(Repository repository)
        {
            this.repository = repository;
        }

        public bool TryGetValue(AuthenticatedStatusEnum key, out IUserService value)
        {
            if (key == AuthenticatedStatusEnum.Authenticated)
            {
                value = new UserService(repository);
            }
            else
            {
                value = new AnonymousUserService(repository);
            }
            return true;
        }

        public IUserService this[AuthenticatedStatusEnum index]
        {
            get
            {
                return index == AuthenticatedStatusEnum.Authenticated 
                    ? new UserService(repository) as IUserService
                    : new AnonymousUserService(repository);
            }
        }

    }
}

