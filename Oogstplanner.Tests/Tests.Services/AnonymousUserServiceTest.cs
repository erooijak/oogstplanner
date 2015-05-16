using System;

using Moq;
using NUnit.Framework;

using Oogstplanner.Data;
using Oogstplanner.Models;
using Oogstplanner.Services;

namespace Oogstplanner.Tests.Services
{
    [TestFixture]
    public class AnonymousUserServiceTest
    {
        [Test]
        public void Services_AnonymousUser_AddUser()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();

            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);
            unitOfWorkMock.SetupGet(mock => mock.Calendars)
                .Returns(calendarRepositoryMock.Object);

            var service = new AnonymousUserService(
                unitOfWorkMock.Object, cookieProviderMock.Object);
                
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
                mock.GetUserByUserName(expectedUserName), 
                Times.Once(),
                "The repository should be called.");
            calendarRepositoryMock.Verify(mock => 
                mock.Add(It.IsAny<Calendar>()), 
                Times.Once(),
                "A calendar should be created.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Exactly(2),
                "Changes should be committed, retrieved for the generated primary key, " +
                "and committed again to database.");
        }

        [Test]
        public void Services_AnonymousUser_GetCurrentUserId()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var expectedCookieValue = Guid.NewGuid().ToString();
            var expectedUserId = new Random().Next();
            var expectedUser = new User { UserId = expectedUserId };

            cookieProviderMock.Setup(mock =>
                mock.GetCookie(It.IsAny<string>()))
                .Returns(expectedCookieValue);
            userRepositoryMock.Setup(mock =>
                mock.GetUserByUserName(expectedCookieValue))
                .Returns(expectedUser);

            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);

            cookieProviderMock.Setup(mock =>
                mock.GetCookie(It.IsAny<string>()))
                .Returns(expectedCookieValue);

            var service = new AnonymousUserService(
                unitOfWorkMock.Object, cookieProviderMock.Object);
                
            // ACT
            var result = service.GetCurrentUserId();

            // ASSERT
            userRepositoryMock.Verify(mock => 
                mock.GetUserByUserName(expectedCookieValue), 
                Times.Once(),
                "When a cookie is available the user should be retrieved.");
            cookieProviderMock.Verify(mock =>
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
            var cookieProviderMock = new Mock<ICookieProvider>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();

            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);
            unitOfWorkMock.SetupGet(mock => mock.Calendars)
                .Returns(calendarRepositoryMock.Object);

            var expectedUserId = new Random().Next();
            var expectedUser = new User { UserId = expectedUserId };

            cookieProviderMock.Setup(mock =>
                mock.GetCookie(It.IsAny<string>()))
                .Returns(string.Empty);
            userRepositoryMock.Setup(mock =>
                mock.GetUserByUserName(It.IsAny<string>()))
                .Returns(expectedUser);

            var service = new AnonymousUserService(
                unitOfWorkMock.Object, cookieProviderMock.Object);

            // ACT
            service.GetCurrentUserId();

            // ASSERT
            cookieProviderMock.Verify(mock =>
                mock.SetCookie(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<double>()),
                Times.Once(),
                "A new cookie should be set.");
            calendarRepositoryMock.Verify(mock => 
                mock.Add(
                    It.Is<Calendar>(c => c.User.Name == expectedUser.Name)), 
                Times.Once(),
                "A new user with calendar should be created.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Exactly(2),
                "Changes should be committed, retrieved for the generated primary key, " +
                "and committed again to database.");
        }

        [Test]
        public void Services_AnonymousUser_GetUser()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();

            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);

            var expectedUserId = new Random().Next();
            var expectedUser = new User { UserId = expectedUserId };

            userRepositoryMock.Setup(mock =>
                mock.GetById(expectedUserId))
                .Returns(expectedUser);

            var service = new AnonymousUserService(
                unitOfWorkMock.Object, cookieProviderMock.Object);

            // ACT
            var result = service.GetUser(expectedUserId);

            // ASSERT
            Assert.AreEqual(expectedUser, result,
                "The user with the specified id should be retrieved.");

        }

        [Test]
        public void Services_AnonymousUser_NoCommitsOnQueries()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var expectedCookieValue = Guid.NewGuid().ToString();
            var expectedUserId = new Random().Next();
            var expectedUser = new User { UserId = expectedUserId };

            cookieProviderMock.Setup(mock =>
                mock.GetCookie(It.IsAny<string>()))
                .Returns(expectedCookieValue);
            userRepositoryMock.Setup(mock =>
                mock.GetUserByUserName(expectedCookieValue))
                .Returns(expectedUser);

            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);

            cookieProviderMock.Setup(mock =>
                mock.GetCookie(It.IsAny<string>()))
                .Returns(expectedCookieValue);

            var service = new AnonymousUserService(
                unitOfWorkMock.Object, cookieProviderMock.Object);

            // ACT
            service.GetCurrentUserId();
            service.GetUser(It.IsAny<int>());

            // ASSERT
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Never,
                "No changes to commit to database on queries.");
        }
    }
}
