using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public interface IFarmingActionRepository : IRepository<FarmingAction>
    {
        IEnumerable<FarmingAction> GetFarmingActions(Expression<Func<FarmingAction, bool>> predicate);
        FarmingAction FindRelated(FarmingAction action);
        Month GetMonthsWithAction(int userId);
    }
}
    