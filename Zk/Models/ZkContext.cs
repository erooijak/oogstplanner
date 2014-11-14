using System.Data.Entity;

namespace Zk.Models
{
	public class ZkContext : DbContext, IZkContext
	{

		#region Constructors

		/// <summary>
		///     Initializes a new instance of the <see cref="Zk.Models.ZkContext"/> class.
		/// </summary>
		public ZkContext() : base("ZkDatabaseConnection")
		{
		}

		#endregion

		#region Database tables

		public IDbSet<Crop> Crops { get; set; } 

		#endregion
	}
}