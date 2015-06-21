using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

using Moq;
using NUnit.Framework;

using Oogstplanner.Common;
using Oogstplanner.Models;
using Oogstplanner.Services;
using Oogstplanner.Tests.Lib;
using Oogstplanner.Web.Controllers;

namespace Oogstplanner.Tests.Controllers
{
    [TestFixture]
    public class CalendarControllerTest
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
        public void Controllers_Calendar_SowingAndHarvesting_DisplayMonthOrdering()
        {
            // ARRANGE
            var expectedMonthOrdering = GetExpectedMonthOrdering();

            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

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
        public void Controllers_Calendar_SowingAndHarvesting_DataMonthOrdering()
        {
            // ARRANGE
            var expectedMonthOrdering = GetExpectedMonthOrdering();

            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

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
        public void Controllers_Calendar_SowingAndHarvesting_HasActions()
        {
            // ARRANGE
            var expectedMonthOrdering = GetExpectedMonthOrdering();

            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();
            calendarServiceMock.Setup(c => c.GetMonthsWithAction())
                .Returns(Months.April | Months.August);

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

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

        [Test]
        public void Controllers_Calendar_Month()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            var expectedMonth = Months.April.ToString();
            var expectedMonthCalendarViewModel = new MonthCalendarViewModel
            {
                DisplayMonth = expectedMonth
            };
            calendarServiceMock.Setup(mock => 
                mock.GetMonthCalendar(Months.April))
                .Returns(expectedMonthCalendarViewModel);
            
            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

            // ACT
            var viewResult = controller.Month(Months.April) as PartialViewResult;

            // ASSERT
            Assert.AreEqual("~/Views/Calendar/_MonthCalendar.cshtml", viewResult.ViewName,
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
                    DisplayMonth = Months.May.ToString(),
                    HarvestingActions = new[] { new FarmingAction() },
                    SowingActions = new List<FarmingAction>()
                });
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
        public void Controllers_Calendar_YearForUser()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            var expectedYearCalendarViewModel = new YearCalendarViewModel();

            expectedYearCalendarViewModel.Add(new MonthCalendarViewModel 
                { 
                    DisplayMonth = Months.May.ToString(),
                    HarvestingActions = new[] { new FarmingAction() },
                    SowingActions = new List<FarmingAction>()
                });
            calendarServiceMock.Setup(mock => 
                mock.GetYearCalendar(It.IsAny<string>()))
                .Returns(expectedYearCalendarViewModel);

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

            // ACT
            var viewResult = controller.YearForUser("test") as ViewResult;

            // ASSERT
            Assert.AreEqual(expectedYearCalendarViewModel, (YearCalendarViewModel)viewResult.Model,
                "Model should be the viewmodel returned by the service.");
        }

        [Test]
        public void Controllers_Calendar_YearForUserNotFound()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            calendarServiceMock.Setup(mock => 
                mock.GetYearCalendar(It.IsAny<string>()))
                .Throws<UserNotFoundException>();

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

            controller.SetMockControllerContext();

            // ACT
            var viewResult = controller.YearForUser("test") as ViewResult;

            // ASSERT
            Assert.IsTrue(viewResult.ViewName.Contains("UserDoesNotExist"),
                "If user cannot be found user does not exist view should be returned.");
            Assert.AreEqual(404, controller.Response.StatusCode,
                "With a 404 status code.");
        }
            
        [Test]
        public void Controllers_Calendar_YearForUserNoActions()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            var expectedYearCalendarViewModel = new YearCalendarViewModel();

            expectedYearCalendarViewModel.Add(new MonthCalendarViewModel 
                { 
                    DisplayMonth = Months.May.ToString(),
                    HarvestingActions = new List<FarmingAction>(),
                    SowingActions = new List<FarmingAction>()
                });
            calendarServiceMock.Setup(mock => 
                mock.GetYearCalendar(It.IsAny<string>()))
                .Returns(expectedYearCalendarViewModel);

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

            const string expectedUserName = "test";

            // ACT
            var viewResult = controller.YearForUser(expectedUserName) as ViewResult;

            // ASSERT
            Assert.AreEqual("NoCropsOtherUser", viewResult.ViewName,
                "The no crops for other user view should should be returned by the controller when the" +
                "view model contains no crops, since the calendar for another user is" +
                "requested.");
            Assert.AreEqual(expectedYearCalendarViewModel, ((YearCalendarViewModel)viewResult.Model),
                "The returned year calendar view model should be returned.");
        }

        [Test]
        public void Controllers_Calendar_YearForUserNoActionsOwnCalendar()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            var expectedYearCalendarViewModel = new YearCalendarViewModel 
                {
                    IsOwnCalendar = true
                };

            expectedYearCalendarViewModel.Add(new MonthCalendarViewModel 
                { 
                    DisplayMonth = Months.May.ToString(),
                    HarvestingActions = new List<FarmingAction>(),
                    SowingActions = new List<FarmingAction>(),
                });
            calendarServiceMock.Setup(mock => 
                mock.GetYearCalendar(It.IsAny<string>()))
                .Returns(expectedYearCalendarViewModel);

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

            // ACT
            var viewResult = controller.YearForUser("test") as ViewResult;

            // ASSERT
            Assert.AreEqual("NoCrops", viewResult.ViewName,
                "The no crops view should should be returned by the controller when the" +
                "view model contains no crops, since the user's own calendar is requested" +
                "via the calendar for another user route");
        }

        [Test]
        public void Controllers_Calendar_YearNoActions()
        {
            // ARRANGE
            var calendarServiceMock = new Mock<ICalendarService>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var cropProviderMock = new Mock<ICropProvider>();

            var expectedYearCalendarViewModel = new YearCalendarViewModel();
            expectedYearCalendarViewModel.Add(new MonthCalendarViewModel 
                { 
                    DisplayMonth = Months.May.ToString(),
                    HarvestingActions = new List<FarmingAction>(),
                    SowingActions = new List<FarmingAction>()
                });
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
            Assert.AreEqual("NoCrops", viewResult.ViewName,
                "The no crops should should be returned by the controller when the" +
                "view model contains no crops.");
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
                    actualIntInCollection, 
                    actualIntInCollection))
                .Throws(new IndexOutOfRangeException());

            var controller = new CalendarController(
                calendarServiceMock.Object,
                farmingActionServiceMock.Object,
                cropProviderMock.Object
            );

            // ACT
            var viewResult = controller.UpdateMonth(formCollection);

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
                Months.May, 
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
                Months.May, 
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
                .Returns(Months.June | Months.July);

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
