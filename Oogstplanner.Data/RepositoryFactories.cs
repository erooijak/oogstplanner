using System;
using System.Collections.Generic;

namespace Oogstplanner.Data
{
    public class RepositoryFactories
    {
        public Dictionary<Type, Func<IOogstplannerContext, object>> Get()
        {
            return new Dictionary<Type, Func<IOogstplannerContext, object>>
            {
                { typeof(CalendarRepository), db => new CalendarRepository(db) },
                { typeof(FarmingActionRepository), db => new FarmingActionRepository(db) },
                { typeof(PasswordRecoveryRepository), db => new PasswordRecoveryRepository(db) },
                { typeof(UserRepository), db => new UserRepository(db) },
                { typeof(CropRepository), db => new CropRepository(db) },
                { typeof(LikesRepository), db => new LikesRepository(db) }
            };
        }
    }
}
