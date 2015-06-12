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
    public class FriendsControllerTest
    {       
        [Test]
        public void Controllers_Friends_GetLikesAmount()
        {
            // ARRANGE
            var calendarLikingServiceMock = new Mock<ICalendarLikingService>();
            calendarLikingServiceMock.Setup(mock =>
                mock.GetLikes(It.IsAny<int>()))
                .Returns(new[] { new Like() });

            var controller = new FriendsController(calendarLikingServiceMock.Object);

            // ACT
            var contentResult = controller.GetLikesCount(It.IsAny<int>()) as ContentResult;

            // ASSERT
            Assert.AreEqual("1", contentResult.Content,
                "One should be returned since there is one like returned from the service.");
        }


        [Test]
        public void Controllers_Friends_UnLikeSuccess()
        {
            // ARRANGE
            var calendarLikingServiceMock = new Mock<ICalendarLikingService>();
            bool wasUnlike = true;
            calendarLikingServiceMock.Setup(mock =>
                mock.Like(It.IsAny<int>(), out wasUnlike));

            var controller = new FriendsController(calendarLikingServiceMock.Object);

            // ACT
            var viewResult = controller.Like(It.IsAny<int>());

            // ASSERT
            calendarLikingServiceMock
                .Verify(mock => 
                    mock.Like(It.IsAny<int>(), out wasUnlike), 
                    Times.Once, 
                    "The Like method should be called once.");
            Assert.IsTrue(viewResult.Data.ToString().Contains("success = True"),
                "If the retrieval is successful the controller should return this.");
            Assert.IsTrue(viewResult.Data.ToString().Contains("wasUnlike = True"),
                "This was an unlike so the controller should return this.");
        }

        [Test]
        public void Controllers_Friends_UnLikeFailure()
        {
            // ARRANGE
            var calendarLikingServiceMock = new Mock<ICalendarLikingService>();
            bool wasUnlike;
            calendarLikingServiceMock.Setup(mock => mock.Like(It.IsAny<int>(), out wasUnlike))
                .Throws<Exception>();

            var controller = new FriendsController(calendarLikingServiceMock.Object);

            // ACT
            var viewResult = controller.Like(It.IsAny<int>());

            // ASSERT
            Assert.IsTrue(viewResult.Data.ToString().Contains("success = False"),
                "If the retrieval is unsuccessful the controller should return this.");
        }

        [Test]
        public void Controllers_Friends_LikeSucces()
        {
            // ARRANGE
            var calendarLikingServiceMock = new Mock<ICalendarLikingService>();

            var controller = new FriendsController(calendarLikingServiceMock.Object);

            // ACT
            var viewResult = controller.Like(It.IsAny<int>());

            // ASSERT
            bool wasUnlike;
            calendarLikingServiceMock
                .Verify(mock => 
                    mock.Like(It.IsAny<int>(), out wasUnlike), 
                    Times.Once, 
                    "The Like method should be called once.");
            Assert.IsTrue(viewResult.Data.ToString().Contains("success = True"),
                "If the retrieval is successful the controller should return this.");
        }

        [Test]
        public void Controllers_Friends_LikeFailure()
        {
            // ARRANGE
            var calendarLikingServiceMock = new Mock<ICalendarLikingService>();
            bool wasUnlike;
            calendarLikingServiceMock.Setup(mock => mock.Like(It.IsAny<int>(), out wasUnlike))
                .Throws<Exception>();

            var controller = new FriendsController(calendarLikingServiceMock.Object);

            // ACT
            var viewResult = controller.Like(It.IsAny<int>()) as JsonResult;

            // ASSERT
            Assert.IsTrue(viewResult.Data.ToString().Contains("success = False"),
                "If the retrieval is unsuccessful the controller should return this.");
        }

    }
}
