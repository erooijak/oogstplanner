using System.Data.Entity;

using Oogstplanner.Common;
using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public class OogstplannerContext : DbContext, IOogstplannerContext
    {
        public IDbSet<Crop> Crops { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public IDbSet<Calendar> Calendars { get; set; }
        public IDbSet<FarmingAction> FarmingActions { get; set; }

        public OogstplannerContext() 
            : base(string.Format("name={0}", ConfigurationHelper.ConnectionStringName))
        {
        }

        IDbSet<T> IOogstplannerContext.Set<T>()
        {
            return base.Set<T>();
        }
            
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // PostgreSQL uses schema public by default.
            modelBuilder.HasDefaultSchema("public");
        }

        public bool IsDeleted(object entity)
        {
            return Entry(entity).State == EntityState.Deleted;
        }

        public bool IsDetached(object entity)
        {
            return Entry(entity).State == EntityState.Detached;
        }

        public void SetAdded(object entity)
        {
            Entry(entity).State = EntityState.Added;
        }

        public void SetDeleted(object entity)
        {
            Entry(entity).State = EntityState.Deleted;
        }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}
