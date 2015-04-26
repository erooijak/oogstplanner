using System;
using System.Threading;
using System.Web;
using System.Collections.Generic;
using System.Linq;

using Zk.Helpers;
using Zk.Models;
using Zk.Repositories;

using Autofac.Features.Indexed;

namespace Zk.Services
{
    public class FarmingActionService
    {
        readonly Repository repository;
        readonly IUserService userService;

        public FarmingActionService(Repository repository, 
            AuthenticationService authService,
            IIndex<AuthenticatedStatus, IUserService> userServices)
        {
            this.repository = repository;
            this.userService = userServices[authService.GetAuthenticationStatus()];
        }

        int? currentUserId;
        public int CurrentUserId 
        { 
            get 
            {
                if (currentUserId == null) 
                {
                    currentUserId = userService.GetCurrentUserId();
                }
                return (int)currentUserId;
            }
        }
            
        public IEnumerable<FarmingAction> GetHarvestingActions(int userId, Month month)
        {
            return repository.GetFarmingActions(fa => fa.Calendar.UserId == userId
                && fa.Action == ActionType.Harvesting 
                && fa.Month.HasFlag(month));
        }
            
        public IEnumerable<FarmingAction> GetSowingActions(int userId, Month month)
        {
            return repository.GetFarmingActions(fa => fa.Calendar.UserId == userId
                && fa.Action == ActionType.Sowing 
                && fa.Month.HasFlag(month));
        }
            
        public void AddAction(FarmingAction farmingAction)
        {
            // Create the related farmingaction (the sowing or harvesting counter part)
            var relatedFarmingAction = CreateRelatedAction(farmingAction);

            // Check if the calendar actually belongs to the current user.
            CheckAuthorisation(CurrentUserId, relatedFarmingAction.Calendar.UserId);

            // Try to see if there is a farming action of the same user, of the same type, of the same crop
            AddOrUpdateAction(farmingAction);
            AddOrUpdateAction(relatedFarmingAction);

            repository.SaveChanges();
        }
            
        public void RemoveAction(int id)
        {
            // Get the farming action.
            var farmingAction = repository.FindFarmingAction(id);

            // Check if the calendar actually belongs to the current user.
            CheckAuthorisation(CurrentUserId, farmingAction.Calendar.UserId);

            // Find the related farmingaction (the sowing or harvesting counter part).
            var relatedFarmingAction = repository.FindRelatedFarmingAction(farmingAction);

            repository.RemoveFarmingAction(farmingAction);
            repository.RemoveFarmingAction(relatedFarmingAction);

            repository.SaveChanges();
        }

        public void UpdateCropCounts(IList<int> ids, IList<int> counts)
        {
            if (ids.Count != counts.Count) throw new ArgumentException(
                "Different amount of ids and counts.", "counts");

            // Combine each farming id to its respective farming count in a key-value pair (kvp),
            // where the id is the key and the crop count the value.
            foreach (var kvp in ids.Zip(counts, (id, count) => new KeyValuePair<int, int>(id, count)))
            {
                var action = repository.FindFarmingAction(kvp.Key);

                // Check if the calendar actually belongs to the current user.
                CheckAuthorisation(CurrentUserId, action.Calendar.UserId);

                // Add if action is new (till end):
                var currentCropCount = action.CropCount;
                var newCropCount = kvp.Value;

                //   Do nothing if value has not changed.
                if (currentCropCount == newCropCount) continue;

                //   Find the related farmingaction (the sowing or harvesting counter part)
                var relatedFarmingAction = repository.FindRelatedFarmingAction(action);

                //   Update the crop count of the farming action and related farming action in the database.
                action.CropCount = relatedFarmingAction.CropCount = newCropCount;
                repository.Update(action, relatedFarmingAction);
            }

            repository.SaveChanges();
        }

        /// <summary>
        ///     This method updates a farming action's crop count if a similar one already exists,
        ///     or adds a new one if it does not.
        /// </summary>
        /// <param name="farmingAction">Farming action.</param>
        void AddOrUpdateAction(FarmingAction farmingAction)
        {
            var existingFarmingAction = repository.GetFarmingActions(
                fa => fa.Action == farmingAction.Action 
                    && fa.Calendar.UserId == farmingAction.Calendar.UserId 
                    && fa.Crop.Id == farmingAction.Crop.Id
                    && fa.Month == farmingAction.Month)
                .FirstOrDefault();

            if (existingFarmingAction == null) 
            {
                repository.AddFarmingAction(farmingAction);
            }
            else 
            {
                existingFarmingAction.CropCount += farmingAction.CropCount;
                repository.Update(existingFarmingAction);
            }

        }

        FarmingAction CreateRelatedAction(FarmingAction action)
        {
            var crop = action.Crop;
            var cropGrowingTime = action.Crop.GrowingTime;
            var calendar = action.Calendar;
            var count = action.CropCount;

            var actionType = FarmingActionHelper.GetRelatedActionType(action);
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