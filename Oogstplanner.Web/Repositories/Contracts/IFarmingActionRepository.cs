using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Oogstplanner.Models;

namespace Oogstplanner.Repositories
{
    public interface IFarmingActionRepository : IRepositoryBase
    {
        IEnumerable<FarmingAction> GetFarmingActions(Expression<Func<FarmingAction, bool>> predicate);

        FarmingAction FindFarmingAction(int id);

        FarmingAction FindRelatedFarmingAction(FarmingAction action);

        void AddFarmingAction(FarmingAction farmingAction);

        void RemoveFarmingAction(FarmingAction farmingAction);
    }
}
    