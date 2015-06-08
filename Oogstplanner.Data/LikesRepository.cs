using System;
using System.Collections.Generic;
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

        public IEnumerable<Like> Find(Expression<Func<Like, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList<Like>();
        }
    }
}
