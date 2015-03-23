using Zk.Models;

namespace Zk.Helpers
{
    public static class FarmingActionHelper
    {
        // This method finds the counter part of the type and month of the inputted action
        public static void SetRelatedTypeAndMonth(FarmingAction action, int cropGrowingTime, out ActionType type, out Month month)
        {
            if (action.Action == ActionType.Harvesting) 
            {
                type = ActionType.Sowing;
                month = action.Month.Subtract(cropGrowingTime);
            }
            else 
            {
                type = ActionType.Harvesting;
                month = action.Month.Add(cropGrowingTime);
            }
        }
    }
}

