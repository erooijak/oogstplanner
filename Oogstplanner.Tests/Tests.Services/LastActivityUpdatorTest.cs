using System;

using Moq;
using NUnit.Framework;

using Oogstplanner.Data;
using Oogstplanner.Models;
using Oogstplanner.Services;

namespace Oogstplanner.Tests.Services
{
    [TestFixture]
    public class LastActivityUpdatorTest
    {
        [Test]
        public void Services_LastActivity_UpdateLastActivity()
        {
            // ARRANGE
            var userRepositoryMock = new Mock<IUserRepository>();

            const int expectedUserId = 1;

            userRepositoryMock.Setup(mock =>
                mock.GetById(It.IsAny<int>()))
                .Returns(new User { Id = expectedUserId });

            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            unitOfWorkMock.SetupGet(mock => mock.Users)
                .Returns(userRepositoryMock.Object);

            var service = new LastActivityUpdator(unitOfWorkMock.Object);

            // ACT
            service.UpdateLastActivity(expectedUserId);

            // ASSERT
            userRepositoryMock.Verify(mock =>
                mock.Update(It.Is<User>(
                    u => u.LastActive.Date == DateTime.Today
                    && u.Id == expectedUserId)), Times.Once,
                "The last activity should be updated to today" +
                "for the user with the specified id");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Once,
                "Changes should be committed.");
        }
    }
}
