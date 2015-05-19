using System.Collections.Generic;

namespace Oogstplanner.Models
{
    /// <summary>
    /// View model used for displaying the details of the month.
    /// </summary>
    public class MonthCalendarViewModel
    {
        public string DisplayMonth { get; set; }
        public IEnumerable<FarmingAction> HarvestingActions { get; set; }
        public IEnumerable<FarmingAction> SowingActions { get; set; }
    }
}