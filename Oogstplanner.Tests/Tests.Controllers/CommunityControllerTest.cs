using System;
using System.Web.Mvc;

using Moq;
using NUnit.Framework;

using Oogstplanner.Models;
using Oogstplanner.Services;
using Oogstplanner.Web.Controllers;

namespace Oogstplanner.Tests.Controllers
{
    [TestFixture]
    public class CommunityControllerTest
    {       
        [Test]
        public void Controllers_Community_GetLikesAmount()
        {
            // ARRANGE
            var calendarLikingServiceMock = new Mock<ICalendarLikingService>();
            var communityServiceMock = new Mock<ICommunityService>();
            calendarLikingServiceMock.Setup(mock =>
                mock.GetLikes(It.IsAny<int>()))
                .Returns(new[] { new Like() });

            var controller = new CommunityController(
                calendarLikingServiceMock.Object,
                communityServiceMock.Object);

            // ACT
            var contentResult = controller.GetLikesCount(It.IsAny<int>()) as ContentResult;

            // ASSERT
            Assert.AreEqual("1", contentResult.Content,
                "One should be returned since there is one like returned from the service.");
        }

        [Test]
        public void Controllers_Community_GetLikesUsers()
        {
            // ARRANGE
            var calendarLikingServiceMock = new Mock<ICalendarLikingService>();
            var communityServiceMock = new Mock<ICommunityService>();

            const string expectedName1 = "test name 1";
            const string expectedName2 = "test name 2";
            var expectedLikes = new[] 
                { 
                    new Like { User = new User { Name = expectedName1 } },
                    new Like { User = new User { Name = expectedName2 } }
                };

            calendarLikingServiceMock.Setup(mock =>
                mock.GetLikes(It.IsAny<int>()))
                .Returns(expectedLikes);

            var controller = new CommunityController(
                calendarLikingServiceMock.Object,
                communityServiceMock.Object);

            // ACT
            var contentResult = controller.GetLikesUserNames(It.IsAny<int>());

            // ASSERT
            Assert.AreEqual(
                "[\"" + expectedName1 + "\",\"" + expectedName2 + "\"]", 
                contentResult.Content,
                "The JSON string should contain user names of both users returned by " +
                "the liking service.");
        }

        [Test]
        public void Controllers_Community_UnLikeSuccess()
        {
            // ARRANGE
            var calendarLikingServiceMock = new Mock<ICalendarLikingService>();
            var communityServiceMock = new Mock<ICommunityService>();
            bool wasUnlike = true;
            calendarLikingServiceMock.Setup(mock =>
                mock.ToggleLike(It.IsAny<int>(), out wasUnlike));

            var controller = new CommunityController(
                calendarLikingServiceMock.Object,
                communityServiceMock.Object);

            // ACT
            var viewResult = controller.Like(It.IsAny<int>());

            // ASSERT
            calendarLikingServiceMock
                .Verify(mock => 
                    mock.ToggleLike(It.IsAny<int>(), out wasUnlike), 
                    Times.Once, 
                    "The Like method should be called once.");
            Assert.IsTrue(viewResult.Data.ToString().Contains("success = True"),
                "If the retrieval is successful the controller should return this.");
            Assert.IsTrue(viewResult.Data.ToString().Contains("wasUnlike = True"),
                "This was an unlike so the controller should return this.");
        }

        [Test]
        public void Controllers_Community_UnLikeFailure()
        {
            // ARRANGE
            var calendarLikingServiceMock = new Mock<ICalendarLikingService>();
            var communityServiceMock = new Mock<ICommunityService>();
            bool wasUnlike;
            calendarLikingServiceMock.Setup(mock => mock.ToggleLike(It.IsAny<int>(), out wasUnlike))
                .Throws<Exception>();

            var controller = new CommunityController(
                calendarLikingServiceMock.Object,
                communityServiceMock.Object);

            // ACT
            var viewResult = controller.Like(It.IsAny<int>());

            // ASSERT
            Assert.IsTrue(viewResult.Data.ToString().Contains("success = False"),
                "If the retrieval is unsuccessful the controller should return this.");
        }

        [Test]
        public void Controllers_Community_LikeSucces()
        {
            // ARRANGE
            var calendarLikingServiceMock = new Mock<ICalendarLikingService>();
            var communityServiceMock = new Mock<ICommunityService>();

            var controller = new CommunityController(
                calendarLikingServiceMock.Object,
                communityServiceMock.Object);

            // ACT
            var viewResult = controller.Like(It.IsAny<int>());

            // ASSERT
            bool wasUnlike;
            calendarLikingServiceMock
                .Verify(mock => 
                    mock.ToggleLike(It.IsAny<int>(), out wasUnlike), 
                    Times.Once, 
                    "The Like method should be called once.");
            Assert.IsTrue(viewResult.Data.ToString().Contains("success = True"),
                "If the retrieval is successful the controller should return this.");
        }

        [Test]
        public void Controllers_Community_LikeFailure()
        {
            // ARRANGE
            var calendarLikingServiceMock = new Mock<ICalendarLikingService>();
            var communityServiceMock = new Mock<ICommunityService>();
            bool wasUnlike;
            calendarLikingServiceMock.Setup(mock => mock.ToggleLike(It.IsAny<int>(), out wasUnlike))
                .Throws<Exception>();

            var controller = new CommunityController(
                calendarLikingServiceMock.Object,
                communityServiceMock.Object);

            // ACT
            var viewResult = controller.Like(It.IsAny<int>()) as JsonResult;

            // ASSERT
            Assert.IsTrue(viewResult.Data.ToString().Contains("success = False"),
                "If the retrieval is unsuccessful the controller should return this.");
        }

        [Test]
        public void Controllers_Community_Search()
        {
            // ARRANGE
            var calendarLikingServiceMock = new Mock<ICalendarLikingService>();
            var communityServiceMock = new Mock<ICommunityService>();

            var controller = new CommunityController(
                calendarLikingServiceMock.Object,
                communityServiceMock.Object);

            const string expectedSearchTerm = "testttt";

            // ACT
            var viewResult = controller.Search(
                expectedSearchTerm,
                new Random().Next(1,10),
                new Random().Next(1,10));

            // ASSERT
            communityServiceMock.Verify(mock =>
                mock.SearchUsers(expectedSearchTerm),
                Times.Once,
                "Users with the specific search term should be searched.");
            Assert.AreEqual("", viewResult.ViewBag.SearchDescription,
                "No search description (information after paging) should be displayed on the page.");
            Assert.AreEqual(expectedSearchTerm, viewResult.ViewBag.SearchTerm,
                "Search term should be returned to page.");
        }

        [Test]
        public void Controllers_Community_Index()
        {
            // ARRANGE
            var calendarLikingServiceMock = new Mock<ICalendarLikingService>();
            var communityServiceMock = new Mock<ICommunityService>();

            var controller = new CommunityController(
                calendarLikingServiceMock.Object,
                communityServiceMock.Object);

            // ACT
            var viewResult = controller.Index(
                new Random().Next(1,10),
                new Random().Next(1,10)) as ViewResult;

            // ASSERT
            communityServiceMock.Verify(mock =>
                mock.GetRecentlyActiveUsers(It.IsAny<int>()),
                Times.Once,
                "Recently active users should be retrieved");
            Assert.AreEqual("laatst actieve gebruikers", 
                viewResult.ViewBag.SearchDescription,
                "Search term should be returned to page.");
        }

    }
}
