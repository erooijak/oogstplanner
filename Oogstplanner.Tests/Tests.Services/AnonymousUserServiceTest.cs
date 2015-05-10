using Moq;
using NUnit.Framework;

using System;

using Oogstplanner.Repositories;
using Oogstplanner.Services;
using Oogstplanner.Models;

namespace Oogstplanner.Tests.Services
{
    [TestFixture]
    public class AnonymousUserServiceTest
    {
        [Test]
        public void Services_AnonymousUser_AddUser()
        {
            // ARRANGE
            var userRepositoryMock = new Mock<IUserRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var cookieProviderMock = new Mock<ICookieProvider>();

            var service = new AnonymousUserService(
                userRepositoryMock.Object, 
                calendarRepositoryMock.Object, 
                cookieProviderMock.Object);
                
            const string expectedUserName = "Test";
            const string expectedFullName = "Test Test";
            const string expectedEmail = "jan@oogstplanner.nl";

            // ACT
            service.AddUser(
                expectedUserName,
                expectedFullName,
                expectedEmail);

            // ASSERT
            userRepositoryMock
                .Verify(mock => 
                    mock.GetUserByUserName(expectedUserName), 
                    Times.Once(),
                    "The repository should be called.");
            calendarRepositoryMock
                .Verify(mock => 
                    mock.CreateCalendar(It.IsAny<User>()), 
                    Times.Once(),
                    "A calendar should be created..");
        }

        [Test]
        public void Services_AnonymousUser_GetCurrentUserId()
        {
            // ARRANGE
            var userRepositoryMock = new Mock<IUserRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var cookieProviderMock = new Mock<ICookieProvider>();

            var expectedCookieValue = Guid.NewGuid().ToString();
            var expectedUserId = new Random().Next();
            var expectedUser = new User { UserId = expectedUserId };

            cookieProviderMock.Setup(mock =>
                mock.GetCookie(It.IsAny<string>()))
                .Returns(expectedCookieValue);
            userRepositoryMock.Setup(mock =>
                mock.GetUserByUserName(expectedCookieValue))
                .Returns(expectedUser);

            var service = new AnonymousUserService(
                userRepositoryMock.Object, 
                calendarRepositoryMock.Object, 
                cookieProviderMock.Object);
                
            // ACT
            var result = service.GetCurrentUserId();

            // ASSERT
            userRepositoryMock
                .Verify(mock => 
                    mock.GetUserByUserName(expectedCookieValue), 
                    Times.Once(),
                    "When a cookie is available the user should be retrieved.");
            cookieProviderMock
                .Verify(mock =>
                    mock.SetCookie(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<double>()),
                    Times.Never(),
                    "A new cookie should not be set.");
            Assert.AreEqual(expectedUserId, result,
                "The user id of the user should be retrieved.");
        }

        [Test]
        public void Services_AnonymousUser_GetCurrentUserIdNoCookie()
        {
            // ARRANGE
            var userRepositoryMock = new Mock<IUserRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var cookieProviderMock = new Mock<ICookieProvider>();

            var expectedUserId = new Random().Next();
            var expectedUser = new User { UserId = expectedUserId };

            cookieProviderMock.Setup(mock =>
                mock.GetCookie(It.IsAny<string>()))
                .Returns(string.Empty);
            userRepositoryMock.Setup(mock =>
                mock.GetUserByUserName(It.IsAny<string>()))
                .Returns(expectedUser);

            var service = new AnonymousUserService(
                userRepositoryMock.Object, 
                calendarRepositoryMock.Object, 
                cookieProviderMock.Object);

            // ACT
            service.GetCurrentUserId();

            // ASSERT
            cookieProviderMock
                .Verify(mock =>
                    mock.SetCookie(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<double>()),
                Times.Once(),
                "A new cookie should be set.");
            calendarRepositoryMock
                .Verify(mock => 
                    mock.CreateCalendar(It.IsAny<User>()), 
                    Times.Once(),
                    "A new user with calendar should be created.");
        }

        [Test]
        public void Services_AnonymousUser_GetUser()
        {
            // ARRANGE
            var userRepositoryMock = new Mock<IUserRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var cookieProviderMock = new Mock<ICookieProvider>();

            var expectedUserId = new Random().Next();
            var expectedUser = new User {UserId = expectedUserId };

            userRepositoryMock.Setup(mock =>
                mock.GetUserById(expectedUserId))
                .Returns(expectedUser);

            var service = new AnonymousUserService(
                userRepositoryMock.Object, 
                calendarRepositoryMock.Object, 
                cookieProviderMock.Object);

            // ACT
            var result = service.GetUser(expectedUserId);

            // ASSERT
            Assert.AreEqual(expectedUser, result,
                "The user with the specified id should be retrieved.");
        }

    }
}
