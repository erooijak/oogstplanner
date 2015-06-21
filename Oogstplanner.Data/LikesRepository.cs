using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public class LikesRepository : EntityFrameworkRepository<Like>, ILikesRepository
    {
        public LikesRepository(IOogstplannerContext db) 
            : base(db)
        { }

        public Like SingleOrDefault(Expression<Func<Like, bool>> predicate)
        {
            return DbSet.SingleOrDefault(predicate);
        }

        public IEnumerable<Like> GetByCalendarId(int calendarId)
        {
            return DbSet.Where(l => l.Calendar.Id == calendarId).ToList();
        }

        public void DeleteLikesBelongingToUser(int userId)
        {
            var likes = DbSet.Where(l => l.User.Id == userId || l.Calendar.User.Id == userId);

            foreach (var like in likes)
            {
                DbSet.Remove(like);
            }
        }
    }
}
