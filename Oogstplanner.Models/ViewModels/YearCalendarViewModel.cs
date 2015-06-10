﻿using System.Collections.ObjectModel;

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
    }
}
