using Zk.Models;

namespace Zk.Helpers
{
    public static class FarmingActionHelper
    {

        public static void GetRelatedActionType(FarmingAction action, int cropGrowingTime)
        {
            return action.Action == ActionType.Harvesting ? ActionType.Sowing : ActionType.Harvesting;
        }
            
        public static void GetRelatedMonth(FarmingAction action, int cropGrowingTime)
        {
            return action.Action == ActionType.Harvesting 
                ? action.Month.Subtract(cropGrowingTime)
                : action.Month.Add(cropGrowingTime);
        }
    }
}

