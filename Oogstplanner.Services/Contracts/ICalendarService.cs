using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public interface ICalendarService
    {
        Calendar GetCalendar();
        YearCalendarViewModel GetYearCalendar();
        MonthCalendarViewModel GetMonthCalendar(Month month);
        Month GetMonthsWithAction();
    }
}
