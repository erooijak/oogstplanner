using Moq;
using NUnit.Framework;

using System;

using Oogstplanner.Repositories;
using Oogstplanner.Services;
using Oogstplanner.Models;
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
    }
}
