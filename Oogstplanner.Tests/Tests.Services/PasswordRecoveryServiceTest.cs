using Moq;
using NUnit.Framework;

using System;

using Oogstplanner.Repositories;
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
            var passwordRecoveryRepositoryMock = new Mock<IPasswordRecoveryRepository>();

            var service = new PasswordRecoveryService(passwordRecoveryRepositoryMock.Object);

            const string expectedEmail = "test@oogstplanner.nl";
            var expectedToken = Guid.NewGuid().ToString();

            // ACT
            service.StoreResetToken(expectedEmail, expectedToken);

            // ASSERT
            passwordRecoveryRepositoryMock.Verify(mock =>
                mock.StoreResetToken(expectedEmail, 
                    It.Is<DateTime>(dt => dt.Date == DateTime.Today),
                    expectedToken), 
                    Times.Once,
                "The token and e-mail should be set and time of reset should be this day.");
        }

        [Test]
        public void Services_PasswordRecovery_GetUserFromToken()
        {
            // ARRANGE
            var passwordRecoveryRepositoryMock = new Mock<IPasswordRecoveryRepository>();

            var service = new PasswordRecoveryService(passwordRecoveryRepositoryMock.Object);
                
            var expectedToken = Guid.NewGuid().ToString();

            // ACT
            service.GetMembershipUserFromToken(expectedToken);

            // ASSERT
            passwordRecoveryRepositoryMock.Verify(mock =>
                mock.GetMembershipUserFromToken(expectedToken), 
                Times.Once,
                "The repository should be called with the expected token.");
        }

        [Test]
        public void Services_PasswordRecovery_GetUserFromMail()
        {
            // ARRANGE
            var passwordRecoveryRepositoryMock = new Mock<IPasswordRecoveryRepository>();

            var service = new PasswordRecoveryService(passwordRecoveryRepositoryMock.Object);

            const string expectedEmail = "test@oogstplanner.nl";

            // ACT
            service.GetMembershipUserByEmail(expectedEmail);

            // ASSERT
            passwordRecoveryRepositoryMock.Verify(mock =>
                mock.GetMembershipUserByEmail(expectedEmail), 
                Times.Once,
                "The repository should be called with the expected e-mail.");
        }
      
        [Test]
        public void Services_PasswordRecovery_GetTokenTimestamp()
        {
            // ARRANGE
            var passwordRecoveryRepositoryMock = new Mock<IPasswordRecoveryRepository>();

            var service = new PasswordRecoveryService(passwordRecoveryRepositoryMock.Object);

            var expectedToken = Guid.NewGuid().ToString();

            // ACT
            service.GetTokenTimeStamp(expectedToken);

            // ASSERT
            passwordRecoveryRepositoryMock.Verify(mock =>
                mock.GetTokenTimeStamp(expectedToken), 
                Times.Once,
                "The repository should be called with the expected token.");
        }

        [Test]
        public void Services_PasswordRecovery_GenerateToken()
        {
            // ARRANGE
            var passwordRecoveryRepositoryMock = new Mock<IPasswordRecoveryRepository>();

            var service = new PasswordRecoveryService(passwordRecoveryRepositoryMock.Object);

            // ACT
            var result = service.GenerateToken();

            // ASSERT
            Assert.IsNotEmpty(result, "A token should be generated");
            Assert.Greater(result.Length, 20, "Length of token should be at least 20 chars to " +
                "have a higher guarantee of uniqueness");
        }
    }
}
