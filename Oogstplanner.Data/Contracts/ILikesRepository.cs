using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public interface ILikesRepository : IRepository<Like>
    {
        Like SingleOrDefault(Expression<Func<Like, bool>> predicate);
        IEnumerable<Like> GetByCalendarId(int calendarId);
        void DeleteLikesBelongingToUser(int userId);
    }
}
