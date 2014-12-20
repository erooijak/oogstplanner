using System.Linq;
using Zk.Models;
using System.Collections.Generic;
using System;

namespace Zk.Repositories
{
	/// <summary>
	///     Repository used for methods that access the database ... and some business logic.
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

        public IEnumerable<FarmingAction> GetHarvestingActions(Month month)
        {
            return _db.FarmingActions.Where(
                fm => fm.Action == FarmType.Harvesting && fm.Month.HasFlag(month))
                    .ToList();
        }

        public IEnumerable<FarmingAction> GetSowingActions(Month month)
        {
            return _db.FarmingActions.Where(
                fm => fm.Action == FarmType.Sowing && fm.Month.HasFlag(month))
                    .ToList();
        }
            
        public void UpdateCropCounts(IList<int> ids, IList<int> counts)
        {
            if (ids.Count != counts.Count) throw new ArgumentException(
                "Different amount of ids and counts.", "counts");

            // Combine each farming id to it's respective farming count in a keyvaluepair (kvp)
            // where the id is the key and cropCount the value.
            foreach (var kvp in ids.Zip(counts, (id, count) => new KeyValuePair<int, int>(id, count)))
            {
                var action = _db.FarmingActions.Find(kvp.Key);
                if (action == null) 
                    throw new ArgumentException("Cannot find primary key in database.", "ids");

                var oldCropCount = action.CropCount;
                var newCropCount = kvp.Value;

                if (oldCropCount == newCropCount) continue;

                // TODO:    Implement logic to update all related farming actions.
                // AKA:     The super calculation.

                // Update one crop count of a farming action in the database.
                action.CropCount = kvp.Value;
                _db.SetModified(action);
                _db.SaveChanges();

            }
        }

	}
}