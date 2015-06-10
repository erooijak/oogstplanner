using System.Collections.Generic;

using NUnit.Framework;

using Oogstplanner.Models;

namespace Oogstplanner.Tests.Models
{
    [TestFixture]
    public class YearCalendarViewModelTest
    {
        [Test]
        public void Models_YearCalendarViewModelTestHasAnyActions()
        {
            // ARRANGE
            var vm = new YearCalendarViewModel();
            vm.Add(new MonthCalendarViewModel 
                { 
                    HarvestingActions = new[] { new FarmingAction() }, 
                    SowingActions = new List<FarmingAction>() 
                });

            // ACT
            var hasAnyActions = vm.HasAnyActions();

            // ASSERT
            Assert.IsTrue(hasAnyActions,
                "The view model has one action so HasAnyActions should return true.");
        }

        [Test]
        public void Models_YearCalendarViewModelTestHasAnyActions_Empty()
        {
            // ARRANGE
            var vm = new YearCalendarViewModel();
            vm.Add(new MonthCalendarViewModel 
                { 
                    HarvestingActions = new List<FarmingAction>(), 
                    SowingActions = new List<FarmingAction>() 
                });

            // ACT
            var hasAnyActions = vm.HasAnyActions();

            // ASSERT
            Assert.IsFalse(hasAnyActions,
                "The view model has no actions so HasAnyActions should return false.");
        }
    }
}
