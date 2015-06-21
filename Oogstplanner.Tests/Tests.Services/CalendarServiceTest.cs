using System;
using System.Collections.Generic;

using Moq;
using NUnit.Framework;

using Oogstplanner.Data;
using Oogstplanner.Models;
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
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            unitOfWorkMock.SetupGet(mock =>
                mock.Calendars).Returns(calendarRepositoryMock.Object);

            var fakeUserServices = new FakeUserServices();
            fakeUserServices.ReturnedUserService = 
                new UserService(unitOfWorkMock.Object, 
                    new Mock<ICookieProvider>().Object,
                    new Mock<ILastActivityUpdator>().Object);

            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var expectedUserId = new Random().Next();
            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            var service = new CalendarService(
                unitOfWorkMock.Object, 
                farmingActionServiceMock.Object,
                fakeUserServices,
                authenticationServiceMock.Object);
                
            // ACT
            service.GetCalendar();

            // ASSERT
            calendarRepositoryMock.Verify(mock =>
                mock.GetByUserId(expectedUserId), 
                    Times.Once,
                "The calendar for the user id should be retrieved");
        }

        [Test]
        public void Services_Calendar_GetYearCalendar()
        {
            // ARRANGE
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var calendarUserId = new Random().Next();
            calendarRepositoryMock.Setup(mock =>
                mock.GetByUserId(It.IsAny<int>()))
                .Returns(new Calendar
                { 
                    Id = 1, 
                    User = new User { Id = calendarUserId }, 
                    Likes = new List<Like>() 
                });

            unitOfWorkMock.SetupGet(mock =>
                mock.Calendars).Returns(calendarRepositoryMock.Object);

            var fakeUserServices = new FakeUserServices();
            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var expectedUserId = new Random().Next();
            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            var service = new CalendarService(
                unitOfWorkMock.Object, 
                farmingActionServiceMock.Object,
                fakeUserServices,
                authenticationServiceMock.Object);

            // ACT
            YearCalendarViewModel result = service.GetYearCalendar();

            // ASSERT
            farmingActionServiceMock.Verify(mock =>
                mock.GetHarvestingActions(expectedUserId, It.IsAny<Months>()),
                Times.Exactly(12),
                "12 month calendars with harvesting actions should be retrieved " +
                "for a year calendar.");
            farmingActionServiceMock.Verify(mock =>
                mock.GetSowingActions(expectedUserId, It.IsAny<Months>()),
                Times.Exactly(12),
                "12 month calendars with sowing actions should be retrieved " +
                "for a year calendar.");
            Assert.IsTrue(result.IsOwnCalendar,
                "The calendar should be set as belonging to the user who requested it.");
        }

        [Test]
        public void Services_Calendar_GetYearCalendarForOtherUser()
        {
            // ARRANGE
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            const string expectedUserName = "test";

            const int calendarUserId = 123456;
            calendarRepositoryMock.Setup(mock =>
                mock.GetByUserId(It.IsAny<int>()))
                .Returns(new Calendar
                    { 
                        Id = 1, 
                        User = new User { Id = calendarUserId }, 
                        Likes = new List<Like>() 
                    });

            unitOfWorkMock.SetupGet(mock =>
                mock.Calendars).Returns(calendarRepositoryMock.Object);

            var fakeUserServices = new FakeUserServices();
            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var expectedUserId = new Random().Next();
            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);
            userServiceMock.Setup(mock => mock.GetUserByName(expectedUserName))
                .Returns(new User { Id = expectedUserId });

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            var service = new CalendarService(
                unitOfWorkMock.Object, 
                farmingActionServiceMock.Object,
                fakeUserServices,
                authenticationServiceMock.Object);

            // ACT
            YearCalendarViewModel result = service.GetYearCalendar(expectedUserName);

            // ASSERT
            farmingActionServiceMock.Verify(mock =>
                mock.GetHarvestingActions(expectedUserId, It.IsAny<Months>()),
                Times.Exactly(12),
                "12 month calendars with harvesting actions should be retrieved " +
                "for a year calendar.");
            farmingActionServiceMock.Verify(mock =>
                mock.GetSowingActions(expectedUserId, It.IsAny<Months>()),
                Times.Exactly(12),
                "12 month calendars with sowing actions should be retrieved " +
                "for a year calendar.");
            Assert.IsFalse(result.IsOwnCalendar,
                "The calendar should not be set as belonging to the user who requested it" +
                "since the user id on the calendar is different from the user id of the user.");
            Assert.AreEqual(expectedUserName, result.UserName, 
                "The user name should be set to the user name on the calendar");
        }

        [Test]
        public void Services_Calendar_GetYearCalendarForOtherUserThatIsTheUserHisOrHerSelf()
        {
            // ARRANGE
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            const string expectedUserName = "test";

            const int calendarUserId = 123456;
            calendarRepositoryMock.Setup(mock =>
                mock.GetByUserId(It.IsAny<int>()))
                .Returns(new Calendar
                    { 
                        Id = 1, 
                        User = new User { Id = calendarUserId }, 
                        Likes = new List<Like>() 
                    });

            unitOfWorkMock.SetupGet(mock =>
                mock.Calendars).Returns(calendarRepositoryMock.Object);

            var fakeUserServices = new FakeUserServices();
            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            const int expectedUserId = calendarUserId;
            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);
            userServiceMock.Setup(mock => mock.GetUserByName(expectedUserName))
                .Returns(new User { Id = expectedUserId });

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            var service = new CalendarService(
                unitOfWorkMock.Object, 
                farmingActionServiceMock.Object,
                fakeUserServices,
                authenticationServiceMock.Object);

            // ACT
            YearCalendarViewModel result = service.GetYearCalendar(expectedUserName);

            // ASSERT
            Assert.IsTrue(result.IsOwnCalendar,
                "The calendar should be set as belonging to the user who requested it" +
                "since the user id on the calendar is the same as the user id of the requesting user.");
        }
            
        [Test]
        public void Services_Calendar_GetMonthCalendar()
        {
            // ARRANGE
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var fakeUserServices = new FakeUserServices();
            fakeUserServices.ReturnedUserService = 
                new UserService(unitOfWorkMock.Object, 
                    new Mock<ICookieProvider>().Object,
                    new Mock<ILastActivityUpdator>().Object);
            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var expectedUserId = new Random().Next();
            var expectedMonth = Months.May;
            var expectedHarvestingActions = new List<FarmingAction>();
            var expectedSowingActions = new List<FarmingAction>();

            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            farmingActionServiceMock.Setup(mock =>
                mock.GetHarvestingActions(
                    expectedUserId,
                    It.IsAny<Months>()))
                .Returns(expectedHarvestingActions);

            farmingActionServiceMock.Setup(mock =>
                mock.GetSowingActions(
                    expectedUserId,
                    It.IsAny<Months>()))
                .Returns(expectedSowingActions);

            var service = new CalendarService(
                unitOfWorkMock.Object, 
                farmingActionServiceMock.Object,
                fakeUserServices,
                authenticationServiceMock.Object);

            // ACT
            MonthCalendarViewModel result = service.GetMonthCalendar(expectedMonth);

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
        public void Services_Calendar_GetMonthCalendarFailure()
        {
            // ARRANGE
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var fakeUserServices = new FakeUserServices();
            fakeUserServices.ReturnedUserService = 
                new UserService(unitOfWorkMock.Object, 
                    new Mock<ICookieProvider>().Object,
                    new Mock<ILastActivityUpdator>().Object);
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var service = new CalendarService(
                unitOfWorkMock.Object, 
                farmingActionServiceMock.Object,
                fakeUserServices,
                authenticationServiceMock.Object);

            var expectedMonths = Months.June | Months.April;

            // ACT AND ASSERT
            Assert.Throws<ArgumentException>(() => service.GetMonthCalendar(expectedMonths),
                "When multiple months are selected the get month calendar method should throw" +
                "an ArgumentException.");
        }

        [Test]
        public void Services_Calendar_GetMonthWithAction()
        {
            // ARRANGE
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var farmingActionRepositoryMock = new Mock<IFarmingActionRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            unitOfWorkMock.SetupGet(mock =>
                mock.FarmingActions).Returns(farmingActionRepositoryMock.Object);

            var fakeUserServices = new FakeUserServices();
            fakeUserServices.ReturnedUserService = 
                new UserService(unitOfWorkMock.Object, 
                    new Mock<ICookieProvider>().Object,
                    new Mock<ILastActivityUpdator>().Object);
            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
           
            var expectedUserId = new Random().Next();

            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            var service = new CalendarService(
                unitOfWorkMock.Object, 
                farmingActionServiceMock.Object,
                fakeUserServices,
                authenticationServiceMock.Object);

            // ACT
            service.GetMonthsWithAction();

            // ASSERT
            farmingActionRepositoryMock.Verify(mock =>
                mock.GetMonthsWithAction(expectedUserId), Times.Once,
                "The get months with action method should be called with the user id.");
        }

        [Test]
        public void Services_Calendar_NoCommitsOnQueries()
        {
            // ARRANGE
            var farmingActionServiceMock = new Mock<IFarmingActionService>();
            var farmingActionRepositoryMock = new Mock<IFarmingActionRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var calendarUserId = new Random().Next();
            calendarRepositoryMock.Setup(mock =>
                mock.GetByUserId(It.IsAny<int>()))
                .Returns(new Calendar
                    { 
                        Id = 1, 
                        User = new User { Id = calendarUserId }, 
                        Likes = new List<Like>() 
                    });

            unitOfWorkMock.SetupGet(mock =>
                mock.FarmingActions).Returns(farmingActionRepositoryMock.Object);
            unitOfWorkMock.SetupGet(mock =>
                mock.Calendars).Returns(calendarRepositoryMock.Object);

            var fakeUserServices = new FakeUserServices();
            fakeUserServices.ReturnedUserService = 
                new UserService(unitOfWorkMock.Object, 
                    new Mock<ICookieProvider>().Object,
                    new Mock<ILastActivityUpdator>().Object);
            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(It.IsAny<int>());

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            var service = new CalendarService(
                unitOfWorkMock.Object, 
                farmingActionServiceMock.Object,
                fakeUserServices,
                authenticationServiceMock.Object);

            // ACT
            service.GetMonthsWithAction();
            service.GetMonthCalendar(It.IsAny<Months>());
            service.GetMonthCalendar(It.IsAny<Months>());
            service.GetYearCalendar();

            // ASSERT
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Never,
                "No changes to commit to database on queries.");
        }
    }
}
