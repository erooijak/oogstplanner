using System.Data.Entity;

namespace Zk.Models
{
	public interface IZkContext
	{
		IDbSet<Crop> Crops { get; }

		int SaveChanges();
	}
}