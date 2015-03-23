using System;
using System.Collections.Generic;
using System.Linq;

using Zk.Helpers;
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
            
        public IEnumerable<FarmingAction> GetHarvestingActions(int userId, Month month)
        {
            return _repository.GetFarmingActions(fa => fa.Calendar.UserId == userId
                && fa.Action == ActionType.Harvesting 
                && fa.Month.HasFlag(month));
        }
            
        public IEnumerable<FarmingAction> GetSowingActions(int userId, Month month)
        {
            return _repository.GetFarmingActions(fa => fa.Calendar.UserId == userId
                && fa.Action == ActionType.Harvesting 
                && fa.Month.HasFlag(month));
        }
            
        public void AddFarmingAction(FarmingAction farmingAction)
        {
            // Create the related farmingaction (the sowing or harvesting counter part)
            var relatedFarmingAction = CreateRelatedFarmingAction(farmingAction);

            _repository.AddFarmingAction(farmingAction);
            _repository.AddFarmingAction(relatedFarmingAction);

            _repository.SaveChanges();
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

                // Add if action is new

                var currentCropCount = action.CropCount;
                var newCropCount = kvp.Value;

                // Do nothing if value has not changed.
                if (currentCropCount == newCropCount) continue;

                // Find the related farmingaction (the sowing or harvesting counter part)
                var relatedFarmingAction = _repository.FindRelatedFarmingAction(action);

                // Update the crop count of the farming action and related farming action in the database.
                action.CropCount = relatedFarmingAction.CropCount = newCropCount;
                _repository.Update(action, relatedFarmingAction);
            }

            _repository.SaveChanges();
        }

        private static FarmingAction CreateRelatedFarmingAction(FarmingAction action)
        {
            // Arrange values to be created
            var crop = action.Crop;
            var cropGrowingTime = action.Crop.GrowingTime;
            var calendar = action.Calendar;
            var count = action.CropCount;
            ActionType actionType;
            Month month;

            FarmingActionHelper.SetRelatedTypeAndMonth(action, cropGrowingTime, out actionType, out month);

            var relatedFarmingAction = new FarmingAction 
            {
                Action = actionType,
                Calendar = calendar,
                Crop = crop,
                CropCount = count,
                Month = month
            };

            return relatedFarmingAction;
        }

    }
}