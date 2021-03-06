﻿using Autofac.Features.Indexed;

using Oogstplanner.Models;
using Oogstplanner.Services;

namespace Oogstplanner.Tests.Lib.Fakes
{
    public class FakeUserServices : IIndex<AuthenticatedStatus, IUserService>
    {
        public IUserService ReturnedUserService { get ; set; }

        public bool TryGetValue(AuthenticatedStatus key, out IUserService value)
        {
            value = ReturnedUserService;

            return true;
        }

        public IUserService this[AuthenticatedStatus index]
        {
            get
            {
                return ReturnedUserService;
            }
        }
    }
}
