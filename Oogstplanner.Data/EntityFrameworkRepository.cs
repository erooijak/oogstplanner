using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Oogstplanner.Data
{
    /// <summary>
    /// The Entity Framework-dependent, generic repository for data access.
    /// </summary>
    public class EntityFrameworkRepository<T> : IRepository<T> where T : class
    {
        protected IOogstplannerContext Db { get; set;}
        protected IDbSet<T> DbSet { get; set; }

        public EntityFrameworkRepository(IOogstplannerContext db)
        {
            if (db == null)
            {
                throw new ArgumentNullException("db");
            }

            Db = db;
            DbSet = db.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return DbSet;
        }

        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            if (!Db.IsDetached(entity))
            {
                Db.SetAdded(entity);
            }
            else
            {
                DbSet.Add(entity);
            }
        }

        public virtual void Update(T entity)
        {
            if (Db.IsDetached(entity))
            {
                DbSet.Attach(entity);
            }  
            Db.SetModified(entity);
        }

        public virtual void Delete(T entity)
        {
            if (!Db.IsDeleted(entity))
            {
                Db.SetDeleted(entity);
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null) // if not found assume already deleted.
            {
                Delete(entity);
            }
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList<T>();
        }

        public virtual void Commit()
        {
            Db.SaveChanges();
        }
    }
}
