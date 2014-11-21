using System.Data.Entity;

namespace Zk.Models
{
	public class ZkContext : DbContext, IZkContext
	{
	
		/// <summary>
		///     Initializes a new instance of the <see cref="Zk.Models.ZkContext"/> class.
		/// </summary>
		public ZkContext() : base("ZkTestDatabaseConnection")
		{
		}

		/// <summary>
		/// This method is called when the model for a derived context has been initialized, but
		///  before the model has been locked down and used to initialize the context. The default
		///  implementation of this method does nothing, but it can be overridden in a derived class
		///  such that the model can be further configured before it is locked down.
		/// </summary>
		/// <remarks></remarks>
		/// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// PostgreSQL uses schema public by default.
			modelBuilder.HasDefaultSchema("public");

		}

		public IDbSet<Crop> Crops { get; set; } 

	}
}