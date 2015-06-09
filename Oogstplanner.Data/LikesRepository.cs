using System;
using System.Linq;
using System.Linq.Expressions;
using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public class LikesRepository : EntityFrameworkRepository<Like>, ILikesRepository
    {
        public LikesRepository(IOogstplannerContext db) : base(db)
        {
        }

        public Like SingleOrDefault(Expression<Func<Like, bool>> predicate)
        {
            return DbSet.SingleOrDefault(predicate);
        }
    }
}
