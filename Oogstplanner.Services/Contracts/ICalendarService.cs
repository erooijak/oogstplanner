using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public interface ICalendarService
    {
        Calendar GetCalendar();
        YearCalendarViewModel GetYearCalendar();
        YearCalendarViewModel GetYearCalendar(string userName);
        MonthCalendarViewModel GetMonthCalendar(Months month);
        Months GetMonthsWithAction();
    }
}
