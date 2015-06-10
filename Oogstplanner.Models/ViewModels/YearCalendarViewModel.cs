using System.Collections.ObjectModel;
using System.Linq;

namespace Oogstplanner.Models
{
    /// <summary>
    /// View model used for displaying the year calendar
    /// </summary>
    public class YearCalendarViewModel : Collection<MonthCalendarViewModel>
    {
        public string UserName { get; set; }
        public int CalendarId { get; set; }
        public int LikesCount { get; set; }
        public bool IsOwnCalendar { get; set; }

        public bool HasAnyActions()
        {
            return this.Any(m => m.SowingActions.Any() || m.HarvestingActions.Any());
        }
    }
}
