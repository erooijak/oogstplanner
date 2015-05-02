using Oogstplanner.Models;
using Oogstplanner.ViewModels;

namespace Oogstplanner.Services
{
    public interface ICalendarService
    {
        int CurrentUserId { get; }

        Calendar GetCalendar();

        YearCalendarViewModel GetYearCalendar();

        MonthCalendarViewModel GetMonthCalendar(Month month);

        Month GetMonthsWithAction();

    }
}