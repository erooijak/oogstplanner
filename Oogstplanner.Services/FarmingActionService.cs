using System;
using System.Collections.Generic;
using System.Linq;

using Autofac.Features.Indexed;

using Oogstplanner.Common;
using Oogstplanner.Data;
using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public class FarmingActionService : ServiceBase, IFarmingActionService
    {
        readonly IUserService userService;

        public FarmingActionService(
            IOogstplannerUnitOfWork unitOfWork,
            IIndex<AuthenticatedStatus, IUserService> userServices,
            IAuthenticationService authService) 
            : base(unitOfWork)
        {
            if (userServices == null)
            {
                throw new ArgumentNullException("userServices");
            }
            if (authService == null)
            {
                throw new ArgumentNullException("authService");
            }

            this.UnitOfWork = unitOfWork;
            this.userService = userServices[authService.GetAuthenticationStatus()];
        }

        int? currentUserId;
        protected int CurrentUserId 
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
            return UnitOfWork.FarmingActions.GetFarmingActions(fa => fa.Calendar.User.Id == userId
                && fa.Action == ActionType.Harvesting 
                && fa.Month.HasFlag(month));
        }
            
        public IEnumerable<FarmingAction> GetSowingActions(int userId, Month month)
        {
            return UnitOfWork.FarmingActions.GetFarmingActions(fa => fa.Calendar.User.Id == userId
                && fa.Action == ActionType.Sowing 
                && fa.Month.HasFlag(month));
        }
            
        public void AddAction(FarmingAction farmingAction)
        {
            // Create the related farmingaction (the sowing or harvesting counter part)
            var relatedFarmingAction = farmingAction.CreateRelated();

            // Check if the calendar actually belongs to the current user.
            CheckAuthorisation(CurrentUserId, farmingAction.Calendar.User.Id);

            // Try to see if there is a farming action of the same user, of the same type, of the same crop
            AddOrUpdateAction(farmingAction);
            AddOrUpdateAction(relatedFarmingAction);

            UnitOfWork.Commit();
        }
            
        public void RemoveAction(int id)
        {
            // Get the farming action.
            var farmingAction = UnitOfWork.FarmingActions.GetById(id);

            // Check if the calendar actually belongs to the current user.
            CheckAuthorisation(CurrentUserId, farmingAction.Calendar.User.Id);

            // Find the related farmingaction (the sowing or harvesting counter part).
            var relatedFarmingAction = UnitOfWork.FarmingActions.FindRelated(farmingAction);

            UnitOfWork.FarmingActions.Delete(farmingAction);
            UnitOfWork.FarmingActions.Delete(relatedFarmingAction);

            UnitOfWork.Commit();
        }

        public void UpdateCropCounts(IList<int> ids, IList<int> counts)
        {
            if (ids.Count != counts.Count)
            {
                throw new ArgumentException("Different amount of ids and counts.");
            }

            // Combine each farming id to its respective farming count in a key-value pair (kvp),
            // where the id is the key and the crop count the value.
            foreach (var kvp in ids.Zip(counts, (id, count) => new KeyValuePair<int, int>(id, count)))
            {
                var action = UnitOfWork.FarmingActions.GetById(kvp.Key);

                // Check if the calendar actually belongs to the current user.
                CheckAuthorisation(CurrentUserId, action.Calendar.User.Id);

                // Add if action is new (till end):
                var currentCropCount = action.CropCount;
                var newCropCount = kvp.Value;

                //   Do nothing if value has not changed.
                if (currentCropCount == newCropCount) continue;

                //   Find the related farmingaction (the sowing or harvesting counter part)
                var relatedFarmingAction = UnitOfWork.FarmingActions.FindRelated(action);

                //   Update the crop count of the farming action and related farming action in the database.
                action.CropCount = relatedFarmingAction.CropCount = newCropCount;

                UnitOfWork.FarmingActions.Update(action);
                UnitOfWork.FarmingActions.Update(relatedFarmingAction);
            }

            UnitOfWork.Commit();
        }

        /// <summary>
        /// This method updates a farming action's crop count if a similar one already exists,
        /// or adds a new one if it does not.
        /// </summary>
        /// <remarks>
        /// Does not commit changes.
        /// </remarks>
        /// <param name="farmingAction">Farming action.</param>
        void AddOrUpdateAction(FarmingAction farmingAction)
        {
            var existingFarmingAction = UnitOfWork.FarmingActions.GetFarmingActions(
                fa => fa.Action == farmingAction.Action 
                    && fa.Calendar.User.Id == farmingAction.Calendar.User.Id 
                    && fa.Crop.Id == farmingAction.Crop.Id
                    && fa.Month == farmingAction.Month)
                .FirstOrDefault();

            if (existingFarmingAction == null) 
            {
                UnitOfWork.FarmingActions.Add(farmingAction);
            }
            else 
            {
                existingFarmingAction.CropCount += farmingAction.CropCount;
                UnitOfWork.FarmingActions.Update(existingFarmingAction);
            }                
        }

        /// <summary>
        /// This method throws a <see cref="SecurityException"/> if the action does not belong to the logged in user
        /// </summary>
        /// <param name="currentUserId">The current user.</param>
        /// <param name="calendarUserId">The calendar belonging to the user.</param>
        static void CheckAuthorisation(int currentUserId, int calendarUserId)
        {
            // Check if action belongs to user or someone is messing with us:
            if (currentUserId != calendarUserId)
            {
                throw new SecurityException("The calendar attempted to be updated does not belong to the logged in user.");
            }
        }
    }
}
