using Oogstplanner.Models;

namespace Oogstplanner.Repositories
{
    public interface ICalendarRepository : IRepositoryBase
    {
        Calendar GetCalendar(int userId);

        void CreateCalendar(User user);

        Month GetMonthsWithAction(int userId);
    }
}
