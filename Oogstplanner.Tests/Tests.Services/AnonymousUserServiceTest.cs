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
            var lastActivityUpdatorMock = new Mock<ILastActivityUpdator>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();

            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);
            unitOfWorkMock.SetupGet(mock => mock.Calendars)
                .Returns(calendarRepositoryMock.Object);

            var service = new AnonymousUserService(
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
                mock.Add(It.Is<User>(u => u.Name == expectedUserName)), 
                Times.Once(),
                "The repository should be called.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Once,
                "Changes should be committed.");
        }

        [Test]
        public void Services_AnonymousUser_GetCurrentUserId()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var lastActivityUpdatorMock = new Mock<ILastActivityUpdator>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var expectedCookieValue = Guid.NewGuid().ToString();
            var expectedUserId = new Random().Next();
            var expectedUser = new User { Id = expectedUserId };

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
                unitOfWorkMock.Object, 
                cookieProviderMock.Object,
                lastActivityUpdatorMock.Object);
                
            // ACT
            var result = service.GetCurrentUserId();

            // ASSERT
            userRepositoryMock.Verify(mock => 
                mock.GetUserByUserName(expectedCookieValue), 
                Times.Once(),
                "When a cookie is available the user should be retrieved.");
            Assert.AreEqual(expectedUserId, result,
                "The user id of the user should be retrieved.");
        }

        [Test]
        public void Services_AnonymousUser_GetCurrentUserId_UpdateLastActivity()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var lastActivityUpdatorMock = new Mock<ILastActivityUpdator>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var expectedCookieValue = Guid.NewGuid().ToString();
            var expectedUserId = new Random().Next();
            var expectedUser = new User { Id = expectedUserId };

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
            cookieProviderMock.Verify(mock =>
                mock.SetCookie(
                    It.IsAny<string>(), 
                    It.IsAny<string>(), 
                    It.IsAny<double>()),
                Times.Once,
                "Cookie should be set to expire later. ");
        }

        [Test]
        public void Services_AnonymousUser_GetCurrentUserIdNoCookie()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var lastActivityUpdatorMock = new Mock<ILastActivityUpdator>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var calendarRepositoryMock = new Mock<ICalendarRepository>();

            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);
            unitOfWorkMock.SetupGet(mock => mock.Calendars)
                .Returns(calendarRepositoryMock.Object);

            var expectedUserId = new Random().Next();
            var expectedUser = new User { Id = expectedUserId };

            cookieProviderMock.Setup(mock =>
                mock.GetCookie(It.IsAny<string>()))
                .Returns(string.Empty);
            userRepositoryMock.Setup(mock =>
                mock.GetUserByUserName(It.IsAny<string>()))
                .Returns(expectedUser);

            var service = new AnonymousUserService(
                unitOfWorkMock.Object, 
                cookieProviderMock.Object,
                lastActivityUpdatorMock.Object);

            // ACT
            service.GetCurrentUserId();

            // ASSERT
            cookieProviderMock.Verify(mock =>
                mock.SetCookie(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<double>()),
                Times.Exactly(2),
                "A new cookie should be set (and expiration time updated " +
                "for the logging).");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Once,
                "Changes should be committed.");
        }

        [Test]
        public void Services_AnonymousUser_GetUser()
        {
            // ARRANGE
            var cookieProviderMock = new Mock<ICookieProvider>();
            var lastActivityUpdatorMock = new Mock<ILastActivityUpdator>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();

            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);

            var expectedUserId = new Random().Next();
            var expectedUser = new User { Id = expectedUserId };

            userRepositoryMock.Setup(mock =>
                mock.GetById(expectedUserId))
                .Returns(expectedUser);

            var service = new AnonymousUserService(
                unitOfWorkMock.Object, 
                cookieProviderMock.Object,
                lastActivityUpdatorMock.Object);

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
            var lastActivityUpdatorMock = new Mock<ILastActivityUpdator>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var expectedCookieValue = Guid.NewGuid().ToString();
            var expectedUserId = new Random().Next();
            var expectedUser = new User { Id = expectedUserId };

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
                unitOfWorkMock.Object, 
                cookieProviderMock.Object,
                lastActivityUpdatorMock.Object);

            // ACT
            service.GetCurrentUserId();
            service.GetUser(It.IsAny<int>());

            // ASSERT
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Never,
                "No changes to commit to database on queries.");
        }
    }
}
