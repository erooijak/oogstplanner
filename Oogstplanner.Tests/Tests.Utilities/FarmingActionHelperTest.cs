using NUnit.Framework;

using Oogstplanner.Models;
using Oogstplanner.Utilities.Helpers;

namespace Oogstplanner.Tests.Utilities
{
    [TestFixture]
    public class FarmingActionHelperTest
    {
        [Test]
        public void Utilities_FarmingActionHelper_GetRelatedSowing()
        {
            // ARRANGE
            var action = new FarmingAction { Action = ActionType.Sowing };

            // ACT
            var result = FarmingActionHelper.GetRelatedActionType(action);

            // ASSERT
            Assert.AreEqual(ActionType.Harvesting, result, 
                "Helper should return opposite action of sowing.");
        }

        [Test]
        public void Utilities_FarmingActionHelper_GetRelatedHarvesting()
        {
            // ARRANGE
            var action = new FarmingAction { Action = ActionType.Harvesting };

            // ACT
            var result = FarmingActionHelper.GetRelatedActionType(action);

            // ASSERT
            Assert.AreEqual(ActionType.Sowing, result, 
                "Helper should return opposite action of harvesting.");
        }
            
        [Test]
        public void Utilities_FarmingActionHelper_GetRelatedMonthHarvesting()
        {
            // ARRANGE
            const int cropGrowingTime = 2;
            var action = new FarmingAction { Action = ActionType.Harvesting, Month = Month.April };

            // ACT
            var result = FarmingActionHelper.GetRelatedMonth(action, cropGrowingTime);

            // ASSERT
            Assert.AreEqual(Month.February, result, 
                "Helper should subtract months when going from harvesting to sowing.");
        }

        [Test]
        public void Utilities_FarmingActionHelper_GetRelatedMonthSowing()
        {
            // ARRANGE
            const int cropGrowingTime = 2;
            var action = new FarmingAction { Action = ActionType.Sowing, Month = Month.April };

            // ACT
            var result = FarmingActionHelper.GetRelatedMonth(action, cropGrowingTime);

            // ASSERT
            Assert.AreEqual(Month.June, result, 
                "Helper should add months when going from sowing to harvesting.");
        }

    }
}