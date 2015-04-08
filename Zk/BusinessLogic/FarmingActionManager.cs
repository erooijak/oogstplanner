using System;
using System.Web;
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
        readonly UserManager _userManager;
        readonly int CurrentUserId;

        public FarmingActionManager(IZkContext db)
        {
            _repository = new Repository(db);
            _userManager = new UserManager(db);

            CurrentUserId = _userManager.GetCurrentUserId();
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
                && fa.Action == ActionType.Sowing 
                && fa.Month.HasFlag(month));
        }
            
        public void AddFarmingAction(FarmingAction farmingAction)
        {
            // Create the related farmingaction (the sowing or harvesting counter part)
            var relatedFarmingAction = CreateRelatedFarmingAction(farmingAction);

            // Check if the calendar actually belongs to the current user.
            CheckAuthorisation(CurrentUserId, relatedFarmingAction.Calendar.UserId);

            _repository.AddFarmingAction(farmingAction);
            _repository.AddFarmingAction(relatedFarmingAction);

            _repository.SaveChanges();
        }
            
        public void RemoveFarmingAction(int id)
        {
            // Get the farming action.
            var farmingAction = _repository.FindFarmingAction(id);

            // Check if the calendar actually belongs to the current user.
            CheckAuthorisation(CurrentUserId, farmingAction.Calendar.UserId);

            // Find the related farmingaction (the sowing or harvesting counter part).
            var relatedFarmingAction = _repository.FindRelatedFarmingAction(farmingAction);

            _repository.RemoveFarmingAction(farmingAction);
            _repository.RemoveFarmingAction(relatedFarmingAction);

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

                // Check if the calendar actually belongs to the current user.
                CheckAuthorisation(CurrentUserId, action.Calendar.UserId);

                // Add if action is new (till end):
                var currentCropCount = action.CropCount;
                var newCropCount = kvp.Value;

                //   Do nothing if value has not changed.
                if (currentCropCount == newCropCount) continue;

                //   Find the related farmingaction (the sowing or harvesting counter part)
                var relatedFarmingAction = _repository.FindRelatedFarmingAction(action);

                //   Update the crop count of the farming action and related farming action in the database.
                action.CropCount = relatedFarmingAction.CropCount = newCropCount;
                _repository.Update(action, relatedFarmingAction);
            }

            _repository.SaveChanges();
        }

        static FarmingAction CreateRelatedFarmingAction(FarmingAction action)
        {
            var crop = action.Crop;
            var cropGrowingTime = action.Crop.GrowingTime;
            var calendar = action.Calendar;
            var count = action.CropCount;

            var actionType = FarmingActionHelper.GetRelatedActionType(action, cropGrowingTime);
            var month = FarmingActionHelper.GetRelatedMonth(action, cropGrowingTime);

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

        /// <summary>
        /// This method throws an exception if the action does not belong to the logged in user.
        /// </summary>
        /// <param name="currentUserId">The current user.</param>
        /// <param name="calendarUserId">The calendar belonging to the user.</param>
        void CheckAuthorisation(int currentUserId, int calendarUserId)
        {
            // Check if action belongs to user or someone is messing with us:
            if (currentUserId != calendarUserId)
                throw new SecurityException("The calendar attempted to be updated does not belong to the logged in user.");
        }
    }
}