using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Oogstplanner.Models;
using Oogstplanner.Utilities.Helpers;

namespace Oogstplanner.Repositories
{
    public class FarmingActionRepository : RepositoryBase
    {
        public FarmingActionRepository(IOogstplannerContext db) 
            : base(db)
        {
        }

        public IEnumerable<FarmingAction> GetFarmingActions(Expression<Func<FarmingAction, bool>> predicate)
        {
            return db.FarmingActions.Where(predicate).ToList<FarmingAction>();
        }

        public FarmingAction FindFarmingAction(int id)
        {
            var action = db.FarmingActions.Find(id);
            if (action == null)
                throw new ArgumentException("Cannot find primary key in database.", "id");

            return action;
        }

        /// <summary>
        ///     This methods finds the action that belongs to the one given as a parameter.
        /// </summary>
        /// <example>
        ///     When an action is passed that says we have to harvest a broccoli in May,
        ///     and a broccoli has a growing time of four months,
        ///     this method returns the sowing action of a broccoli of four months ago
        ///     which belongs to the same calendar and user.
        /// </example>
        /// <returns>The related farming action.</returns>
        /// <param name="action"></param>
        public FarmingAction FindRelatedFarmingAction(FarmingAction action)
        {
            // Arrange values to be found
            var crop = action.Crop;
            var cropGrowingTime = action.Crop.GrowingTime;
            var calendar = action.Calendar;

            var actionType = FarmingActionHelper.GetRelatedActionType(action);
            var month = FarmingActionHelper.GetRelatedMonth(action, cropGrowingTime);

            return db.FarmingActions.SingleOrDefault(fa => fa.Calendar.CalendarId == calendar.CalendarId
                && fa.Action == actionType
                && fa.Crop.Id == crop.Id
                && fa.Month == month);
        }

        public void AddFarmingAction(FarmingAction farmingAction)
        {
            db.FarmingActions.Add(farmingAction);
        }

        public void RemoveFarmingAction(FarmingAction farmingAction)
        {
            db.FarmingActions.Remove(farmingAction);
        }
            
    }
}
    