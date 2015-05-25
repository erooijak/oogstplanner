using System.Linq;

using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public class CalendarRepository : EntityFrameworkRepository<Calendar>, ICalendarRepository
    {
        public CalendarRepository(IOogstplannerContext db) : base(db)
        {
        }

        public Calendar GetByUserId(int userId)
        {
            return DbSet.SingleOrDefault(c => c.User.Id == userId);
        }
    }
}
