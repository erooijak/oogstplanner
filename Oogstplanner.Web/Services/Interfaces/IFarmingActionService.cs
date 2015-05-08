using Autofac.Features.Indexed;

using System;
using System.Collections.Generic;
using System.Linq;

using Oogstplanner.Models;
using Oogstplanner.Repositories;
using Oogstplanner.Utilities.CustomExceptions;
using Oogstplanner.Utilities.Helpers;

namespace Oogstplanner.Services
{
    public interface IFarmingActionService
    {           
        IEnumerable<FarmingAction> GetHarvestingActions(int userId, Month month);
        
        IEnumerable<FarmingAction> GetSowingActions(int userId, Month month);
        
        void AddAction(FarmingAction farmingAction);
        
        void RemoveAction(int id);

        void UpdateCropCounts(IList<int> ids, IList<int> counts);

        void AddOrUpdateAction(FarmingAction farmingAction);
    }
}
