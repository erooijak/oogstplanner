using Oogstplanner.Models;
using Oogstplanner.ViewModels;

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
