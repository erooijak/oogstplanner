using System.Linq;
using Zk.Models;
using System.Collections.Generic;

namespace Zk.Repositories
{
	/// <summary>
	///     Repository used for methods that access the database.
	/// </summary>
	public class Repository
	{
		readonly IZkContext _db; // The interface to Entity Framework database context

		/// <summary>
		///     Initializes a new instance of the <see cref="Repositories.Repository"/>class which
		///     makes use of the real Entity Framework context that connects with the database.
		/// </summary>
		public Repository()
		{
			_db = new ZkContext();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Repositories.Repository"/> class which
		///     can make use of a "Fake" Entity Framework context for unit testing purposes.
		/// </summary>
		/// <param name="dbParam">Database context.</param>
		public Repository(IZkContext dbParam)
		{
			_db = dbParam;
		}
			
		public Crop GetCrop(int id)
		{
			var crop = _db.Crops.Single(c => c.Id == id);

			return crop;
		}

		public Crop GetCrop(string name)
		{
			var crop = _db.Crops.Single(c => c.Name == name);

			return crop;
		}

		public IEnumerable<Crop> GetAllCrops()
		{
			var crops = _db.Crops.OrderBy(c => c.Id);

			return crops;
		}

        public IEnumerable<FarmingAction> GetFarmingActions(Month month)
        {
            return _db.FarmingActions.Where(fm => fm.Month.HasFlag(month)).ToList();
        }

	}
}