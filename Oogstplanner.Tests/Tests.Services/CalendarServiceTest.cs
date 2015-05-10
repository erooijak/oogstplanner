using Moq;
using NUnit.Framework;

using System;
using System.Collections.Generic;

using Oogstplanner.Models;
using Oogstplanner.ViewModels;
using Oogstplanner.Repositories;
using Oogstplanner.Services;
using Oogstplanner.Tests.Lib.Fakes;

namespace Oogstplanner.Tests.Services
{
    [TestFixture]
    public class CalendarServiceTest
    {
        [Test]
        public void Services_Calendar_GetCalendar()
        {
            // ARRANGE
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var fakeUserServices = new FakeUserServices(
                userRepositoryMock.Object, 
                calendarRepositoryMock.Object);
            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var expectedUserId = new Random().Next();
            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            var service = new CalendarService(
                calendarRepositoryMock.Object, 
                farmingActionServiceMock.Object,
                fakeUserServices,
                authenticationServiceMock.Object);
                
            // ACT
            service.GetCalendar();

            // ASSERT
            calendarRepositoryMock.Verify(mock =>
                mock.GetCalendar(expectedUserId), Times.Once,
                "The calendar for the user id should be retrieved");
        }

        [Test]
        public void Services_Calendar_GetYearCalendar()
        {
            // ARRANGE
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var fakeUserServices = new FakeUserServices(
                userRepositoryMock.Object, 
                calendarRepositoryMock.Object);
            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var expectedUserId = new Random().Next();

            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            var service = new CalendarService(
                calendarRepositoryMock.Object, 
                farmingActionServiceMock.Object,
                fakeUserServices,
                authenticationServiceMock.Object);

            // ACT
            var result = service.GetYearCalendar();

            // ASSERT
            farmingActionServiceMock.Verify(mock =>
                mock.GetHarvestingActions(expectedUserId, It.IsAny<Month>()),
                Times.Exactly(12),
                "12 month calendars with harvesting actions should be retrieved " +
                "for a year calendar.");
            farmingActionServiceMock.Verify(mock =>
                mock.GetSowingActions(expectedUserId, It.IsAny<Month>()),
                Times.Exactly(12),
                "12 month calendars with sowing actions should be retrieved " +
                "for a year calendar.");
            Assert.IsInstanceOf(typeof(YearCalendarViewModel), result,
                "A YearCalendarViewModel should be returned");
        }

        [Test]
        public void Services_Calendar_GetMonthCalendar()
        {
            // ARRANGE
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var fakeUserServices = new FakeUserServices(
                userRepositoryMock.Object, 
                calendarRepositoryMock.Object);
            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var expectedUserId = new Random().Next();
            var expectedMonth = Month.May;
            var expectedHarvestingActions = new List<FarmingAction>();
            var expectedSowingActions = new List<FarmingAction>();

            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            farmingActionServiceMock.Setup(mock =>
                mock.GetHarvestingActions(
                    expectedUserId,
                    It.IsAny<Month>()))
                .Returns(expectedHarvestingActions);

            farmingActionServiceMock.Setup(mock =>
                mock.GetSowingActions(
                    expectedUserId,
                    It.IsAny<Month>()))
                .Returns(expectedSowingActions);

            var service = new CalendarService(
                calendarRepositoryMock.Object, 
                farmingActionServiceMock.Object,
                fakeUserServices,
                authenticationServiceMock.Object);

            // ACT
            var result = service.GetMonthCalendar(expectedMonth);

            // ASSERT
            Assert.IsInstanceOf(typeof(MonthCalendarViewModel), result, 
                "A MonthCalendarViewModel should be returned.");
            Assert.AreEqual(expectedHarvestingActions, result.HarvestingActions,
                "The harvesting actions retrieved from the service should be in the view model.");
            Assert.AreEqual(expectedSowingActions, result.SowingActions,
                "The harvesting actions retrieved from the service should be in the view model.");
            Assert.IsNotEmpty(result.DisplayMonth,
                "Display month should have a value.");
        }

        [Test]
        public void Services_Calendar_GetMonthWithAction()
        {
            // ARRANGE
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var fakeUserServices = new FakeUserServices(
                userRepositoryMock.Object, 
                calendarRepositoryMock.Object);
            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var expectedUserId = new Random().Next();

            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            var service = new CalendarService(
                calendarRepositoryMock.Object, 
                farmingActionServiceMock.Object,
                fakeUserServices,
                authenticationServiceMock.Object);

            // ACT
            service.GetMonthsWithAction();

            // ASSERT
            calendarRepositoryMock.Verify(mock =>
                mock.GetMonthsWithAction(expectedUserId), Times.Once,
                "The get months with action method should be called with the user id.");
        }

    }
}
