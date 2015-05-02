using NUnit.Framework;
using Moq;

using System.Collections.Generic;
using System.Web.Mvc;

using Oogstplanner.Controllers;
using Oogstplanner.ViewModels;
using Oogstplanner.Services;

namespace Oogstplanner.Tests
{
    [Ignore] // Because of exception that is thrown by the type initializer for Moq.Proxy.CastleProxyFactory
             // which only occurs when running tests in a batch.
    [TestFixture]
    public class HomeControllerTest
    {
        private Stack<MonthViewModel> GetExpectedMonthOrdering()
        {
            return new Stack<MonthViewModel>(new[] 
            {   
                new MonthViewModel("augustus", true), 
                new MonthViewModel("mei", false),
                new MonthViewModel("februari", false),
                new MonthViewModel("november", false),
                new MonthViewModel("juli", false),
                new MonthViewModel("april", true),
                new MonthViewModel("januari", false),
                new MonthViewModel("oktober", false),     
                new MonthViewModel("juni", false),
                new MonthViewModel("maart", false),
                new MonthViewModel("december", false),
                new MonthViewModel("september", false)       
            });  

        }

        [Test]
        public void Controllers_SowingAndHarvesting_MonthOrdering()
        {
            // Arrange
            var expectedMonthOrdering = GetExpectedMonthOrdering();

            var mock = new Mock<ICalendarService>();
            mock.Setup(c => c.GetMonthsWithActions())
                .Returns(It.IsAny<Month>);
            var controller = new HomeController(mock.Object);

            // Act
            var result = (SowingAndHarvestingViewModel)((ViewResult)controller.SowingAndHarvesting()).Model;

            // Assert
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
        public void Controllers_SowingAndHarvesting_HasActions()
        {
            // Arrange
            var expectedMonthOrdering = GetExpectedMonthOrdering();

            var mock = new Mock<ICalendarService>();
            mock.Setup(c => c.GetMonthsWithActions())
                .Returns(Month.April | Month.Augustus);
            var controller = new HomeController(mock.Object);

            // Act
            var result = (SowingAndHarvestingViewModel)((ViewResult)controller.SowingAndHarvesting()).Model;

            // Assert
            while (expectedMonthOrdering.Count != 0)
            {
                var expected = expectedMonthOrdering.Pop().HasActions;
                var actual = result.OrderedMonthViewModels.Pop().HasActions;

                Assert.AreEqual(expected, actual,
                    "The has actios attribute should be equal since the " +
                    "mock returns .");
            }
        }
 
    }
}