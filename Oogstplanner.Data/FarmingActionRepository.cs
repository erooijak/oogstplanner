using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public class FarmingActionRepository : EntityFrameworkRepository<FarmingAction>, IFarmingActionRepository
    {
        public FarmingActionRepository(IOogstplannerContext db) : base(db)
        {
        }

        public IEnumerable<FarmingAction> GetFarmingActions(Expression<Func<FarmingAction, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList<FarmingAction>();
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
        public FarmingAction FindRelated(FarmingAction action)
        {
            // Arrange values to be found
            var relatedFarmingAction = action.CreateRelated();

            return DbSet.SingleOrDefault(fa => 
                fa.Calendar.Id == relatedFarmingAction.Calendar.Id
                && fa.Action == relatedFarmingAction.Action
                && fa.Crop.Id == relatedFarmingAction.Crop.Id
                && fa.Month == relatedFarmingAction.Month);
        }

        public Month GetMonthsWithAction(int userId)
        {
            return DbSet.Where(fa => fa.Calendar.User.Id == userId)
                .Select(fa => fa.Month)
                .Distinct()
                .ToList()
                .Aggregate((Month)0, (acc, month) => acc |= month);
        }
            
    }
}
