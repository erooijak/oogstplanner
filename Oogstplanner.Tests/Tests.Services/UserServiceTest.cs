using System;
using System.IO;
using System.Security.Principal;
using System.Web;

using Moq;
using NUnit.Framework;

using Oogstplanner.Models;
using Oogstplanner.Repositories;
using Oogstplanner.Services;

namespace Oogstplanner.Tests.Services
{
    [TestFixture]
    public class UserServiceTest
    {
        [Ignore] // Test cannot run in isolation because of configuration manager bug.
        [Test]
        public void Services_User_AddUser()
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
                mock.Update(It.Is<Object[]>(u => 
                    ((User)u[0]).AuthenticationStatus == AuthenticatedStatus.Authenticated
                    && ((User)u[0]).Email == expectedEmail
                    && ((User)u[0]).FullName == expectedFullName
                    && ((User)u[0]).Name == expectedUserName)),
                Times.Once,
                "The existing user should be updated with the correct information.");
        }

        [Ignore] // Test cannot run in isolation because of configuration manager bug.
        [Test]
        public void Services_User_AddUser_CookieRemoved()
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

        [Ignore] // Test cannot run in isolation because of configuration manager bug.
        [Test]
        public void Services_User_AddUser_New()
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

        [Test]
        public void Services_User_GetCurrentUserId()
        {
            // ARRANGE
            const string expectedUserName = "User";

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "www.oogstplanner.com", ""),
                new HttpResponse(new StringWriter())
            );
                
            HttpContext.Current.User = new GenericPrincipal(
                new GenericIdentity(expectedUserName),
                new string[0]
            );

            var userRepositoryMock = new Mock<IUserRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var cookieProviderMock = new Mock<ICookieProvider>();

            var service = new UserService(
                userRepositoryMock.Object, 
                calendarRepositoryMock.Object, 
                cookieProviderMock.Object);

            // ACT
            service.GetCurrentUserId();

            // ASSERT
            userRepositoryMock.Verify(mock =>
                mock.GetUserIdByName(expectedUserName),
                Times.Once,
                "The repository method for obtainment of the user id should be called.");
        }

        [Test]
        public void Services_User_GetUser()
        {
            // ARRANGE
            var userRepositoryMock = new Mock<IUserRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var cookieProviderMock = new Mock<ICookieProvider>();

            var service = new UserService(
                userRepositoryMock.Object, 
                calendarRepositoryMock.Object, 
                cookieProviderMock.Object);

            var expectedId = new Random().Next();

            // ACT
            service.GetUser(expectedId);

            // ASSERT
            userRepositoryMock.Verify(mock =>
                mock.GetUserById(expectedId),
                Times.Once,
                "The repository should be called with the correct id.");
        }
    }
}
