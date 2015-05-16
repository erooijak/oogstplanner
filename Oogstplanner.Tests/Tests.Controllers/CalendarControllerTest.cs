using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

using Moq;
using NUnit.Framework;

using Oogstplanner.Models;
using Oogstplanner.Services;
using Oogstplanner.Web.Controllers;

namespace Oogstplanner.Tests.Controllers
{
    [TestFixture]
    public class CalendarControllerTest
    {
        [Test]
        public void Controllers_Calendar_Month()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            var expectedMonth = Month.April.ToString();
            var expectedMonthCalendarViewModel = new MonthCalendarViewModel
            {
                DisplayMonth = expectedMonth
            };
            calendarServiceMock.Setup(mock => 
                mock.GetMonthCalendar(Month.April))
                .Returns(expectedMonthCalendarViewModel);
            
            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

            // ACT
            var viewResult = controller.Month(Month.April) as PartialViewResult;

            // ASSERT
            Assert.AreEqual("~/Views/Home/_MonthCalendar.cshtml", viewResult.ViewName,
                "Month should return the _MonthCalendar.cshtml partial view.");
            Assert.AreEqual(expectedMonth, ((MonthCalendarViewModel)viewResult.Model).DisplayMonth,
                "And model should be the viewmodel returned by the service.");
        }

        [Test]
        public void Controllers_Calendar_Year()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            var expectedYearCalendarViewModel = new YearCalendarViewModel();
            expectedYearCalendarViewModel.Add(new MonthCalendarViewModel 
                { 
                    DisplayMonth = Month.May.ToString() }
                );
            calendarServiceMock.Setup(mock => 
                mock.GetYearCalendar())
                .Returns(expectedYearCalendarViewModel);

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

            // ACT
            var viewResult = controller.Year() as ViewResult;

            // ASSERT
            Assert.AreEqual(expectedYearCalendarViewModel, (YearCalendarViewModel)viewResult.Model,
                "Model should be the viewmodel returned by the service.");
        }

        [Test]
        public void Controllers_Calendar_UpdateMonthSuccess()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            var formCollection = new FormCollection();
            formCollection.Add("action.Id", "1");
            formCollection.Add("action.CropCount", "2");

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );
                
            // ACT
            var viewResult = controller.UpdateMonth(formCollection) as JsonResult;

            IList expectedCropIds = new List<int>();
            expectedCropIds.Add(1);
            IList expectedCropCounts = new List<int>();
            expectedCropCounts.Add(2);

            // ASSERT
            farmingActionServiceMock
                .Verify(mock => 
                    mock.UpdateCropCounts((
                        IList<int>)expectedCropIds, 
                        (IList<int>)expectedCropCounts), 
                    Times.Once, 
                "The update crop counts method should be called.");
            Assert.AreEqual("{ success = True }", viewResult.Data.ToString(),
                "And success is true should be returned.");
        }

        [Test]
        public void Controllers_Calendar_UpdateMonthFailure()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            var formCollection = new FormCollection();
            formCollection.Add("action.Id", "1");
            formCollection.Add("action.CropCount", "1");

            var actualIntInCollection = new List<int>();
            actualIntInCollection.Add(1);

            farmingActionServiceMock.Setup(mock => 
                mock.UpdateCropCounts(
                    (IList<int>)actualIntInCollection, 
                    (IList<int>)actualIntInCollection))
                .Throws(new IndexOutOfRangeException());

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

            // ACT
            var viewResult = controller.UpdateMonth(formCollection) as JsonResult;

            IList expectedCropIds = new List<int>();
            expectedCropIds.Add(1);
            IList expectedCropCounts = new List<int>();
            expectedCropCounts.Add(2);

            // ASSERT
            Assert.AreEqual("{ success = False }", viewResult.Data.ToString(),
                "When the controller fails success should be set to false.");
        }

        [Test]
        public void Controllers_Calendar_RemoveSuccess()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            const int idToRemove = 1;

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

            // ACT
            var viewResult = controller.RemoveFarmingAction(idToRemove) as JsonResult;

            // ASSERT
            farmingActionServiceMock
                .Verify(mock => 
                    mock.RemoveAction(idToRemove), 
                    Times.Once, 
                    "The RemoveAction method should be called.");
            Assert.AreEqual("{ success = True }", viewResult.Data.ToString(),
                "When the controller succeeds succes should be set to true.");
        }

        [Test]
        public void Controllers_Calendar_RemoveFailure()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            const int idToRemove = 1;

            farmingActionServiceMock.Setup(mock => 
                mock.RemoveAction(idToRemove))
                .Throws(new IndexOutOfRangeException());

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

            // ACT
            var viewResult = controller.RemoveFarmingAction(idToRemove) as JsonResult;
           
            // ASSERT
            Assert.AreEqual("{ success = False }", viewResult.Data.ToString(),
                "When the controller fails success should be set to false.");
        }

        [Test]
        public void Controllers_Calendar_AddFarmingActionSuccess()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );
                
            // ACT
            var viewResult = controller.AddFarmingAction(
                1, 
                Month.May, 
                ActionType.Harvesting, 
                2) as JsonResult;

            // ASSERT
            farmingActionServiceMock
                .Verify(mock => 
                    mock.AddAction(It.IsAny<FarmingAction>()), 
                    Times.Once, 
                    "The AddFarmingAction method should be called.");
            Assert.AreEqual("{ success = True }", viewResult.Data.ToString(),
                "When the controller succeeds succes should be set to true.");
        }

        [Test]
        public void Controllers_Calendar_AddFarmingActionFailure()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

            farmingActionServiceMock
                .Setup(mock => 
                    mock.AddAction(It.IsAny<FarmingAction>()))
                .Throws(new ArgumentOutOfRangeException());

            // ACT
            var viewResult = controller.AddFarmingAction(
                1, 
                Month.May, 
                ActionType.Harvesting, 
                2) as JsonResult;

            // ASSERT
            farmingActionServiceMock
                .Verify(mock => 
                    mock.AddAction(It.IsAny<FarmingAction>()), 
                    Times.Once, 
                    "The AddFarmingAction method should be called.");
            Assert.AreEqual("{ success = False }", viewResult.Data.ToString(),
                "When the controller fails succes should be set to false.");
        }

        [Test]
        public void Controllers_Calendar_GetMonthsWithAction()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            calendarServiceMock.Setup(mock =>
                mock.GetMonthsWithAction())
                .Returns(Month.June | Month.July);

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );;

            // ACT
            var result = controller.GetMonthsWithAction() as ContentResult;

            // ASSERT
            Assert.AreEqual("[\"june\",\"july\"]", result.Content,
                "The method should return the two months which are returned by the service.");
        }            
    }
}
