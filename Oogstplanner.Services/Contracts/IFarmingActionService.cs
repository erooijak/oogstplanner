using System.Collections.Generic;

using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public interface IFarmingActionService
    {           
        IEnumerable<FarmingAction> GetHarvestingActions(int userId, Months month);        
        IEnumerable<FarmingAction> GetSowingActions(int userId, Months month);        
        void AddActionPair(FarmingAction farmingAction);        
        void RemoveActionPair(int id);
        void UpdateCropCounts(IList<int> ids, IList<int> counts);
    }
}
