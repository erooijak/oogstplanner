using System.Collections.Generic;

using Moq;
using NUnit.Framework;

using Oogstplanner.Data;
using Oogstplanner.Models;
using Oogstplanner.Services;
using System.Linq.Expressions;
using System;

namespace Oogstplanner.Tests.Services
{
    [TestFixture]
    public class CalendarLikingServiceTest
    {
        [Test]
        public void Services_CalendarLiking_Like()
        {
            // ARRANGE
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var userServiceMock = new Mock<IUserService>();

            const int expectedUserId = 1234;
            var expectedUser = new User { Id = 4567 };

            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);
            userServiceMock.Setup(mock => mock.GetUser(expectedUserId))
                .Returns(expectedUser);

            var expectedLike = new List<Like>();
            expectedLike.Add(new Like { User = new User { Id = expectedUserId } } );

            var expectedCalendar = new Calendar { Likes = expectedLike };
            calendarRepositoryMock.Setup(mock =>
                mock.GetById(It.IsAny<int>()))
                .Returns(expectedCalendar);

            unitOfWorkMock.SetupGet(mock =>
                mock.Calendars).Returns(calendarRepositoryMock.Object);

            var service = new CalendarLikingService(
                unitOfWorkMock.Object, 
                userServiceMock.Object);

            bool wasUnlike = false;

            // ACT
            service.ToggleLike(It.IsAny<int>(), out wasUnlike);

            // ASSERT
            Assert.AreEqual(2, expectedLike.Count, 
                "The like should be added to the calendar");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Exactly(1),
                "Changes should be committed.");
            Assert.IsFalse(wasUnlike, "Since the user did not like the calendar this was not an unlike");
        }

        [Test]
        public void Services_CalendarLiking_LikeAlreadyLikes()
        {
            // ARRANGE
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var likesRepositoryMock = new Mock<ILikesRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var userServiceMock = new Mock<IUserService>();

            const int expectedUserId = 1234;
            var expectedUser = new User { Id = expectedUserId };

            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);
            userServiceMock.Setup(mock => mock.GetUser(expectedUserId))
                .Returns(expectedUser);

            var expectedLike = new[] { new Like { User = new User { Id = expectedUserId } } };

            likesRepositoryMock.Setup(mock =>
                mock.SingleOrDefault(It.IsAny<Expression<Func<Like, bool>>>()))
                .Returns(expectedLike[0]);

            unitOfWorkMock.SetupGet(mock =>
                mock.Likes).Returns(likesRepositoryMock.Object);

            calendarRepositoryMock.Setup(mock =>
                mock.GetById(It.IsAny<int>()))
                .Returns(new Calendar { Likes = expectedLike });

            unitOfWorkMock.SetupGet(mock =>
                mock.Calendars).Returns(calendarRepositoryMock.Object);
                
            var service = new CalendarLikingService(
                unitOfWorkMock.Object, 
                userServiceMock.Object);

            bool wasUnlike;
                
            // ACT
            service.ToggleLike(It.IsAny<int>(), out wasUnlike);

            // ASSERT
            likesRepositoryMock.Verify(mock =>
                mock.Delete(It.IsAny<Like>()), 
                Times.Once,
                "A like should not be deleted from the calendar.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Once,
                "Changes should be committed.");
            Assert.IsTrue(wasUnlike, "Since the user already liked the calendar this was an unlike");
        }
            
        [Test]
        public void Services_CalendarLiking_UnLike_NotFound()
        {
            // ARRANGE
            var calendarRepositoryMock = new Mock<ICalendarRepository>();
            var likesRepositoryMock = new Mock<ILikesRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var userServiceMock = new Mock<IUserService>();

            const int expectedUserId = 1234;
            var expectedUser = new User { Id = 4567 };

            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);
            userServiceMock.Setup(mock => mock.GetUser(expectedUserId))
                .Returns(expectedUser);

            calendarRepositoryMock.Setup(mock => mock.GetById(It.IsAny<int>()))
                .Returns(default(Calendar));

            Like expectedLike = null;

            likesRepositoryMock.Setup(mock =>
                mock.SingleOrDefault(It.IsAny<Expression<Func<Like, bool>>>()))
                .Returns(expectedLike);

            unitOfWorkMock.SetupGet(mock =>
                mock.Calendars).Returns(calendarRepositoryMock.Object);

            unitOfWorkMock.SetupGet(mock =>
                mock.Likes).Returns(likesRepositoryMock.Object);

            var service = new CalendarLikingService(
                unitOfWorkMock.Object, 
                userServiceMock.Object);

            bool wasUnlike;

            // ACT
            service.ToggleLike(It.IsAny<int>(), out wasUnlike);

            // ASSERT
            likesRepositoryMock.Verify(mock => mock.Delete(expectedLike), Times.Never,
                "If not nothing should be deleted.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Never,
                "Changes should not be committed.");
        }

        [Test]
        public void Services_CalendarLiking_GetLikes()
        {
            // ARRANGE
            var likesRepositoryMock = new Mock<ILikesRepository>();
            var userServiceMock = new Mock<IUserService>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            unitOfWorkMock.SetupGet(mock =>
                mock.Likes).Returns(likesRepositoryMock.Object);

            var service = new CalendarLikingService(
                unitOfWorkMock.Object, 
                userServiceMock.Object);

            // ACT
            service.GetLikes(It.IsAny<int>());

            // ASSERT
            likesRepositoryMock.Verify(mock => mock.GetByCalendarId(It.IsAny<int>()), 
                Times.Once,
                "Repository should be called to collect the likes.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Never,
                "No changes should not be committed.");
        }
    }
}
