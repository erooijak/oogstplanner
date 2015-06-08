using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;

using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public interface ILikesRepository : IRepository<Like>
    {
        IEnumerable<Like> Find(Expression<Func<Like, bool>> predicate);
    }
}
