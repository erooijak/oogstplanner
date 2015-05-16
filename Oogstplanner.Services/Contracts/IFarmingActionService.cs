using System.Collections.Generic;

using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public interface IFarmingActionService
    {           
        IEnumerable<FarmingAction> GetHarvestingActions(int userId, Month month);        
        IEnumerable<FarmingAction> GetSowingActions(int userId, Month month);        
        void AddAction(FarmingAction farmingAction);        
        void RemoveAction(int id);
        void UpdateCropCounts(IList<int> ids, IList<int> counts);
    }
}
