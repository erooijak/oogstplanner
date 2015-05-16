using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public interface ICalendarRepository : IRepository<Calendar>
    {
        Calendar GetByUserId(int userId);
    }
}
