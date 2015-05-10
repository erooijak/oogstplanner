using Moq;
using NUnit.Framework;

using System;

using Oogstplanner.Repositories;
using Oogstplanner.Services;
using Oogstplanner.Models;

namespace Oogstplanner.Tests.Services
{
    //[Ignore] // Test cannot run in isolation 
    [TestFixture]
    public class UserServiceTest
    {
        [Test]
        public void Services_AnonymousUser_AddUser()
        {
            // ARRANGE
            var userRepositoryMock = new Mock<IUserRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var cookieProviderMock = new Mock<ICookieProvider>();

            var expectedClientUserName = Guid.NewGuid().ToString();
            const int expectedUserId = 3;
            var existingUser = new User { UserId = expectedUserId };

            cookieProviderMock.Setup(mock =>
                mock.GetCookie(It.IsAny<string>()))
                .Returns(expectedClientUserName);

            userRepositoryMock.Setup(mock =>
                mock.GetUserByUserName(expectedClientUserName))
                .Returns(existingUser);

            var service = new UserService(
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
            userRepositoryMock.Verify(mock =>
                mock.Update(It.Is<User>(u => 
                    u.AuthenticationStatus == AuthenticatedStatus.Authenticated
                    && u.CreationDate.Date == DateTime.Today
                    && u.Email == expectedEmail
                    && u.FullName == expectedFullName
                    && u.Name == expectedUserName)),
                Times.Once,
                "The existing user should be updated with the correct information.");
        }

        [Test]
        public void Services_AnonymousUser_AddUser_CookieRemoved()
        {
            // ARRANGE
            var userRepositoryMock = new Mock<IUserRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var cookieProviderMock = new Mock<ICookieProvider>();

            cookieProviderMock.Setup(mock =>
                mock.GetCookie(It.IsAny<string>()))
                .Returns("Username");

            userRepositoryMock.Setup(mock =>
                mock.GetUserByUserName(It.IsAny<string>()))
                .Returns(new User());

            var service = new UserService(
                userRepositoryMock.Object, 
                calendarRepositoryMock.Object, 
                cookieProviderMock.Object);

            // ACT
            service.AddUser("", "", "");

            // ASSERT
            cookieProviderMock.Verify(mock =>
                mock.RemoveCookie(It.IsAny<string>()),
                Times.Once,
                "The user cookie should be removed after login: " +
                "otherwise the user with the cookie's name cannot be found after logging out");
        }

        [Test]
        public void Services_AnonymousUser_AddUser_New()
        {
            // ARRANGE
            var userRepositoryMock = new Mock<IUserRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var cookieProviderMock = new Mock<ICookieProvider>();

            var expectedClientUserName = Guid.NewGuid().ToString();
            const int expectedUserId = 3;
            var existingUser = new User { UserId = expectedUserId };

            cookieProviderMock.Setup(mock =>
                mock.GetCookie(It.IsAny<string>()))
                .Returns("");

            userRepositoryMock.Setup(mock =>
                mock.GetUserByUserName(expectedClientUserName))
                .Returns(existingUser);

            var service = new UserService(
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
            userRepositoryMock.Verify(mock =>
                mock.AddUser(It.Is<User>(u => 
                    u.AuthenticationStatus == AuthenticatedStatus.Authenticated
                    && u.CreationDate.Date == DateTime.Today
                    && u.Email == expectedEmail
                    && u.FullName == expectedFullName
                    && u.Name == expectedUserName)),
                Times.Once,
                "A new user should be created with the correct information.");
           
            calendarRepositoryMock.Verify(mock =>
                mock.CreateCalendar(It.IsAny<User>()),
                Times.Once,
                "A new user should get a calendar.");
        }

    }
}
