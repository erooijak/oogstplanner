using System;
using System.Collections.Generic;
using System.Linq;

using Zk.Models;
using Zk.Repositories;

namespace Zk.BusinessLogic
{
    public class FarmingActionManager
    {
        readonly Repository _repository;

        public FarmingActionManager()
        {
            _repository = new Repository();
        }

        public FarmingActionManager(Repository repository)
        {
            _repository = repository;
        }

        public IEnumerable<FarmingAction> GetHarvestingActions(Month month)
        {
            return _repository.GetFarmingActions(
                fa => fa.Action == ActionType.Harvesting && fa.Month.HasFlag(month));
        }

        public IEnumerable<FarmingAction> GetSowingActions(Month month)
        {
            return _repository.GetFarmingActions(
                fa => fa.Action == ActionType.Sowing && fa.Month.HasFlag(month));
        }

        public void UpdateCropCounts(IList<int> ids, IList<int> counts)
        {
            if (ids.Count != counts.Count) throw new ArgumentException(
                "Different amount of ids and counts.", "counts");

            // Combine each farming id to its respective farming count in a key-value pair (kvp),
            // where the id is the key and the crop count the value.
            foreach (var kvp in ids.Zip(counts, (id, count) => new KeyValuePair<int, int>(id, count)))
            {
                var action = _repository.FindFarmingAction(kvp.Key);

                var currentCropCount = action.CropCount;
                var newCropCount = kvp.Value;

                // Do nothing if value has not changed.
                if (currentCropCount == newCropCount) continue;

                // Otherwise find and update the related farmingaction
                var relatedFarmingAction = _repository.FindRelatedFarmingAction(action);

                // Update the crop count of the farming action and related farming action in the database.
                action.CropCount = newCropCount;
                relatedFarmingAction.CropCount = newCropCount;

                _repository.Update(action);
            }

            _repository.SaveChanges();
        }

    }
}