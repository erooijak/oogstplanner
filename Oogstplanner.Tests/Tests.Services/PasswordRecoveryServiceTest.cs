using System;

using Moq;
using NUnit.Framework;

using Oogstplanner.Data;
using Oogstplanner.Models;
using Oogstplanner.Services;

namespace Oogstplanner.Tests.Services
{
    [TestFixture]
    public class PasswordRecoveryServiceTest
    {
        [Test]
        public void Services_PasswordRecovery_StoreToken()
        {
            // ARRANGE
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var passwordRecoveryRepositoryMock = new Mock<IPasswordRecoveryRepository>();
            unitOfWorkMock.SetupGet(mock =>
                mock.PasswordRecovery).Returns(passwordRecoveryRepositoryMock.Object);

            var service = new PasswordRecoveryService(unitOfWorkMock.Object);

            const string expectedEmail = "test@oogstplanner.nl";
            var expectedToken = Guid.NewGuid().ToString();

            // ACT
            service.StoreResetToken(expectedEmail, expectedToken);

            // ASSERT
            passwordRecoveryRepositoryMock.Verify(mock =>
                mock.Add(
                    It.Is<PasswordResetToken>(prt => 
                        prt.TimeStamp.Date == DateTime.Today
                        && prt.Token == expectedToken
                        && prt.Email == expectedEmail)),
                    Times.Once,
                "The token and e-mail should be set and time of reset should be this day.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Once,
                "Changes should be committed.");
        }
                    
        [Test]
        public void Services_PasswordRecovery_GetTokenTimestamp()
        {
            // ARRANGE
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var passwordRecoveryRepositoryMock = new Mock<IPasswordRecoveryRepository>();
            unitOfWorkMock.SetupGet(mock =>
                mock.PasswordRecovery).Returns(passwordRecoveryRepositoryMock.Object);

            var service = new PasswordRecoveryService(unitOfWorkMock.Object);

            var expectedToken = Guid.NewGuid().ToString();

            // ACT
            service.GetTokenTimeStamp(expectedToken);

            // ASSERT
            passwordRecoveryRepositoryMock.Verify(mock =>
                mock.GetByToken(expectedToken), 
                Times.Once,
                "The repository should be called with the expected token.");
        }

        [Test]
        public void Services_PasswordRecovery_GenerateToken()
        {
            // ARRANGE
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var service = new PasswordRecoveryService(unitOfWorkMock.Object);

            // ACT
            var result = service.GenerateToken();

            // ASSERT
            Assert.IsNotEmpty(result, "A token should be generated");
            Assert.Greater(result.Length, 20, "Length of token should be at least 20 chars to " +
                "have a higher guarantee of uniqueness");
        }

        [Test]
        public void Services_PasswordRecovery_NoCommitsOnQueries()
        {
            // ARRANGE
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            var passwordRecoveryRepositoryMock = new Mock<IPasswordRecoveryRepository>();
            unitOfWorkMock.SetupGet(mock =>
                mock.PasswordRecovery).Returns(passwordRecoveryRepositoryMock.Object);

            var service = new PasswordRecoveryService(unitOfWorkMock.Object);

            // ACT
            service.GetTokenTimeStamp(It.IsAny<string>());
            service.GenerateToken();

            // ASSERT
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Never,
                "No changes should be committed during queries.");
        }
    }
}
