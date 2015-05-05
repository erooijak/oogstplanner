using System.Linq;

using Oogstplanner.Models;

namespace Oogstplanner.Repositories
{
    public class CalendarRepository : RepositoryBase
    {
        public CalendarRepository(IOogstplannerContext db) 
            : base(db)
        {
        }

        public Calendar GetCalendar(int userId)
        {
            return db.Calendars.SingleOrDefault(c => c.User.UserId == userId);
        }

        public void CreateCalendar(User user)
        {
            var calendar = new Calendar { User = user };

            db.Calendars.Add(calendar);
            db.SaveChanges();
        }

        public Month GetMonthsWithAction(int userId)
        {
            return db.FarmingActions.Where(fa => fa.Calendar.UserId == userId)
                .Select(fa => fa.Month)
                .Distinct()
                .ToList()
                .Aggregate((Month)0, (acc, month) => acc |= month);
        }
    }
}
    