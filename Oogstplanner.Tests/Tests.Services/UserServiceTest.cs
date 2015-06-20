using System;
using System.IO;
using System.Security.Principal;
using System.Web;

using Moq;
using NUnit.Framework;

using Oogstplanner.Data;
using Oogstplanner.Models;
using Oogstplanner.Services;

namespace Oogstplanner.Tests.Services
{
    [TestFixture]
    public class UserServiceTest
    {
        [Ignore] // Test cannot run in batch because of configuration manager error.
        [Test]
        public void Services_User_AddUser()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var lastActivityUpdatorMock = new Mock<ILastActivityUpdator>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var expectedClientUserName = Guid.NewGuid().ToString();
            const int expectedUserId = 3;
            var existingUser = new User { Id = expectedUserId };

            userRepositoryMock.Setup(mock =>
                mock.GetUserByUserName(expectedClientUserName))
                .Returns(existingUser);

            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);

            cookieProviderMock.Setup(mock =>
                mock.GetCookie(It.IsAny<string>()))
                .Returns(expectedClientUserName);

            var service = new UserService(
                unitOfWorkMock.Object, 
                cookieProviderMock.Object,
                lastActivityUpdatorMock.Object);

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
                    && u.Email == expectedEmail
                    && u.FullName == expectedFullName
                    && u.Name == expectedUserName)),
                Times.Once,
                "The existing user should be updated with the correct information.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Once,
                "Changes should be committed only once since an existing user is updated.");
        }

        [Ignore] // Test cannot run in batch because of configuration manager error.
        [Test]
        public void Services_User_AddUser_CookieRemoved()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var lastActivityUpdatorMock = new Mock<ILastActivityUpdator>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var expectedClientUserName = Guid.NewGuid().ToString();
            const int expectedUserId = 3;
            var existingUser = new User { Id = expectedUserId };

            userRepositoryMock.Setup(mock =>
                mock.GetUserByUserName(expectedClientUserName))
                .Returns(existingUser);

            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);

            cookieProviderMock.Setup(mock =>
                mock.GetCookie(It.IsAny<string>()))
                .Returns(expectedClientUserName);
         
            var service = new UserService(
                unitOfWorkMock.Object, 
                cookieProviderMock.Object,
                lastActivityUpdatorMock.Object);

            // ACT
            service.AddUser("", "", "");

            // ASSERT
            cookieProviderMock.Verify(mock =>
                mock.RemoveCookie(It.IsAny<string>()),
                Times.Once,
                "The user cookie should be removed after login: " +
                "otherwise the user with the cookie's name cannot be found after logging out");
        }

        [Ignore] // Test cannot run in batch because of configuration manager error.
        [Test]
        public void Services_User_AddUser_New()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var lastActivityUpdatorMock = new Mock<ILastActivityUpdator>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);
            unitOfWorkMock.SetupGet(mock => mock.Calendars)
                .Returns(calendarRepositoryMock.Object);

            cookieProviderMock.Setup(mock =>
                mock.GetCookie(It.IsAny<string>()))
                .Returns("");

            var service = new UserService(
                unitOfWorkMock.Object, 
                cookieProviderMock.Object,
                lastActivityUpdatorMock.Object);

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
                mock.Add(It.Is<User>(u => 
                    u.AuthenticationStatus == AuthenticatedStatus.Authenticated
                    && u.LastActive.Date == DateTime.Today
                    && u.Email == expectedEmail
                    && u.FullName == expectedFullName
                    && u.Name == expectedUserName)),
                Times.Once,
                "A new user should be created with the correct information.");           
            calendarRepositoryMock.Verify(mock =>
                mock.Add(It.IsAny<Calendar>()),
                Times.Once,
                "The new user should get a calendar.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Exactly(2),
                "Changes should be committed, retrieved for the generated primary key, " +
                "and committed again to database.");
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

            var cookieProviderMock = new Mock<ICookieProvider>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var lastActivityUpdatorMock = new Mock<ILastActivityUpdator>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);

            var service = new UserService(
                unitOfWorkMock.Object, 
                cookieProviderMock.Object,
                lastActivityUpdatorMock.Object);

            // ACT
            service.GetCurrentUserId();

            // ASSERT
            userRepositoryMock.Verify(mock =>
                mock.GetUserIdByName(expectedUserName),
                Times.Once,
                "The repository method for obtainment of the user id should be called.");
        }

        [Test]
        public void Services_User_GetCurrentUserId_UpdateLastActivity()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var lastActivityUpdatorMock = new Mock<ILastActivityUpdator>();
           
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "www.oogstplanner.com", ""),
                new HttpResponse(new StringWriter())
            );

            HttpContext.Current.User = new GenericPrincipal(
                new GenericIdentity("Something"),
                new string[0]
            );

            const int expectedUserId = 1;

            userRepositoryMock.Setup(mock =>
                mock.GetUserIdByName(It.IsAny<string>()))
                .Returns(expectedUserId);

            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);

            var service = new UserService(
                unitOfWorkMock.Object, 
                cookieProviderMock.Object,
                lastActivityUpdatorMock.Object);

            // ACT
            service.GetCurrentUserId();

            // ASSERT
            lastActivityUpdatorMock.Verify(mock =>
                mock.UpdateLastActivity(expectedUserId),
                Times.Once,
                "The update last activity method should be called with the " +
                "current user's id every time the current user id is retrieved.");
        }

        [Test]
        public void Services_User_GetUser()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var lastActivityUpdatorMock = new Mock<ILastActivityUpdator>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);
           
            var service = new UserService(
                unitOfWorkMock.Object, 
                cookieProviderMock.Object,
                lastActivityUpdatorMock.Object);

            var expectedId = new Random().Next();

            // ACT
            service.GetUser(expectedId);

            // ASSERT
            userRepositoryMock.Verify(mock =>
                mock.GetById(expectedId),
                Times.Once,
                "The repository should be called with the correct id.");
        }

        [Test]
        public void Services_User_RemoveUser()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var lastActivityUpdatorMock = new Mock<ILastActivityUpdator>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);

            var service = new UserService(
                unitOfWorkMock.Object, 
                cookieProviderMock.Object,
                lastActivityUpdatorMock.Object);

            var expectedId = new Random().Next();

            // ACT
            service.RemoveUser(expectedId);

            // ASSERT
            userRepositoryMock.Verify(mock =>
                mock.Delete(expectedId),
                Times.Once,
                "The repository's delete method should be called with the correct id.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Once,
                "Change should be committed.");
        }

        [Test]
        public void Services_User_NoCommitsOnQueries()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var lastActivityUpdatorMock = new Mock<ILastActivityUpdator>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);

            var service = new UserService(
                unitOfWorkMock.Object, 
                cookieProviderMock.Object,
                lastActivityUpdatorMock.Object);

            // ACT
            service.GetUser(It.IsAny<int>());
            service.GetCurrentUserId();

            // ASSERT
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Never,
                "No changes to commit to database on queries.");
        }
    }
}
