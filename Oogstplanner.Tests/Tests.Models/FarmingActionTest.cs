using NUnit.Framework;

using Oogstplanner.Models;

namespace Oogstplanner.Tests.Models
{
    [TestFixture]
    public class FarmingActionTest
    {
        [Test]
        public void Models_FarmingAction_ToRelatedSowing()
        {
            // ARRANGE
            var initialAction = new FarmingAction 
                { 
                    Action = ActionType.Sowing, 
                    Month = Months.April,
                    Crop = new Crop { GrowingTime = 2 }
                };

            // ACT
            var newAction = initialAction.CreateRelated();

            // ASSERT
            Assert.AreEqual(ActionType.Harvesting, newAction.Action, 
                "Should be converted to opposite action of sowing.");
            Assert.AreEqual(Months.June, newAction.Month, 
                "Months should be added when going from sowing to harvesting.");
        }

        [Test]
        public void Models_FarmingAction_ToRelatedHarvesting()
        {
            // ARRANGE
            var initialAction = new FarmingAction 
                { 
                    Action = ActionType.Harvesting, 
                    Month = Months.April,
                    Crop = new Crop { GrowingTime = 2 }
                };

            // ACT
            var newAction = initialAction.CreateRelated();

            // ASSERT
            Assert.AreEqual(ActionType.Sowing, newAction.Action, 
                "Should be converted to opposite action of harvesting.");
            Assert.AreEqual(Months.February, newAction.Month, 
                "Months should be subtracted when going from harvesting to sowing.");
        }
    }
}
