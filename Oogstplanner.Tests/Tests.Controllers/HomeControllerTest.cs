using NUnit.Framework;
using Moq;

using System.Collections.Generic;
using System.Web.Mvc;

using Oogstplanner.Controllers;
using Oogstplanner.ViewModels;
using Oogstplanner.Services;

namespace Oogstplanner.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        static Stack<MonthViewModel> GetExpectedMonthOrdering()
        {
            return new Stack<MonthViewModel>(new[] 
                {   
                    new MonthViewModel("august", "AUGUSTUS", true), 
                    new MonthViewModel("may", "MEI", false),
                    new MonthViewModel("february", "FEBRUARI", false),
                    new MonthViewModel("november", "NOVEMBER", false),
                    new MonthViewModel("july", "JULI", false),
                    new MonthViewModel("april", "APRIL", true),
                    new MonthViewModel("january","JANUARI", false),
                    new MonthViewModel("october", "OKTOBER", false),     
                    new MonthViewModel("june", "JUNI", false),
                    new MonthViewModel("march", "MAART", false),
                    new MonthViewModel("december", "DECEMBER", false),
                    new MonthViewModel("september", "SEPTEMBER", false)       
                });  

        }

        [Test]
        public void Controllers_Home_SowingAndHarvesting_DisplayMonthOrdering()
        {
            // ARRANGE
            var expectedMonthOrdering = GetExpectedMonthOrdering();

            var mock = new Mock<ICalendarService>();
            mock.Setup(c => c.GetMonthsWithAction())
                .Returns(It.IsAny<Month>);
            var controller = new HomeController(mock.Object);

            // ACT
            var result = (SowingAndHarvestingViewModel)((ViewResult)controller.SowingAndHarvesting()).Model;

            // ASSERT
            while (expectedMonthOrdering.Count != 0)
            {
                var expected = expectedMonthOrdering.Pop().MonthForDisplay;
                var actual = result.OrderedMonthViewModels.Pop().MonthForDisplay;

                Assert.AreEqual(expected, actual,
                    "The months for display should be equal since they are " +
                    "ordered in the above way to have proper display.");
            }
        }

        [Test]
        public void Controllers_Home_SowingAndHarvesting_DataMonthOrdering()
        {
            // ARRANGE
            var expectedMonthOrdering = GetExpectedMonthOrdering();

            var mock = new Mock<ICalendarService>();
            mock.Setup(c => c.GetMonthsWithAction())
                .Returns(It.IsAny<Month>);
            var controller = new HomeController(mock.Object);

            // ACT
            var result = (SowingAndHarvestingViewModel)((ViewResult)controller.SowingAndHarvesting()).Model;

            // ASSERT
            while (expectedMonthOrdering.Count != 0)
            {
                var expected = expectedMonthOrdering.Pop().MonthForDataAttribute;
                var actual = result.OrderedMonthViewModels.Pop().MonthForDataAttribute;

                Assert.AreEqual(expected, actual,
                    "The months in the data attributes should be in the correct order and format.");
            }
        }
            
        [Test]
        public void Controllers_Home_SowingAndHarvesting_HasActions()
        {
            // ARRANGE
            var expectedMonthOrdering = GetExpectedMonthOrdering();

            var mock = new Mock<ICalendarService>();
            mock.Setup(c => c.GetMonthsWithAction())
                .Returns(Month.April | Month.August);
            var controller = new HomeController(mock.Object);

            // ACT
            var result = (SowingAndHarvestingViewModel)((ViewResult)controller.SowingAndHarvesting()).Model;

            // ASSERT
            while (expectedMonthOrdering.Count != 0)
            {
                var expected = expectedMonthOrdering.Pop().HasAction;
                var actual = result.OrderedMonthViewModels.Pop().HasAction;

                Assert.AreEqual(expected, actual,
                    "The has action attribute should be equal to April and August " +
                    "since that is returned by the mock.");
            }
        }
 
    }
}
