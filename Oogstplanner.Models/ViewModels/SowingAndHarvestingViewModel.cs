using System.Collections.Generic;

namespace Oogstplanner.Models
{
    /// <summary>
    /// View model used for the seasons and months displayed on view, 
    /// the CSS classes and the JavaScript binding.
    /// </summary>
    public class SowingAndHarvestingViewModel
    {
        public IEnumerable<string> SeasonsCssClasses { get; set; }
        public IEnumerable<string> SeasonsForDisplay { get; set; }
        public IEnumerable<string> MonthNames { get; set; }
        public IEnumerable<string> DisplayMonthNames { get; set; }
        public Stack<MonthViewModel> OrderedMonthViewModels { get; set; }
    }
}
