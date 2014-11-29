using System.Data.Entity;

namespace Zk.Models
{
	public interface IZkContext
	{
		IDbSet<Crop> Crops { get; }
        IDbSet<FarmingMonth> FarmingMonths { get; }
        IDbSet<Calendar> Calendars { get; }
        IDbSet<User> Users { get; }

		int SaveChanges();
	}
}