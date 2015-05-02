using System.Data.Entity;

namespace Oogstplanner.Models
{
	public class OogstplannerContext : DbContext, IOogstplannerContext
	{
	
        /// <summary>
        ///     Initializes a new instance of the <see cref="Oogstplanner.Models.OogstplannerContext"/> class.
        /// </summary>
        public OogstplannerContext() : base("name=TestOogstplannerDatabaseConnection")
        {
        }

        /// <summary>
        ///     This method is called when the model for a derived context has been initialized, but
        ///     before the model has been locked down and used to initialize the context. The default
        ///     implementation of this method does nothing, but it can be overridden in a derived class
        ///     such that the model can be further configured before it is locked down.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // PostgreSQL uses schema public by default.
            modelBuilder.HasDefaultSchema("public");

        }

		public IDbSet<Crop> Crops { get; set; } 
        public IDbSet<User> Users { get; set; }
        public IDbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public IDbSet<Calendar> Calendars { get; set; }
        public IDbSet<FarmingAction> FarmingActions { get; set; }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

    }
}