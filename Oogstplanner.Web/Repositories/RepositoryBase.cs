using System;

using Oogstplanner.Models;

namespace Oogstplanner.Repositories
{
    /// <summary>
    ///     Repository used for methods that access the database.
    /// </summary>
    public abstract class RepositoryBase
    {
        protected readonly IOogstplannerContext db; // Entity Framework database context

        /// <summary>
        ///     Initializes a new instance of the <see cref="Repositories.RepositoryBase"/> class which
        ///     can make use of a "Fake" Entity Framework context for unit testing purposes.
        /// </summary>
        /// <param name="db">Database context.</param>
        protected RepositoryBase(IOogstplannerContext db)
        {
            this.db = db;
        }

        public void Update(params object[] entities)
        {
            foreach (var entity in entities) db.SetModified(entity);
        }

        // A "leaky abstraction", why not use context directly?
        public void SaveChanges()
        {
            db.SaveChanges();
        }

    }
}
