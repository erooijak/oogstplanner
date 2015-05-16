using System.Security.Principal;
using System.Threading;

using Moq;
using NUnit.Framework;

using Oogstplanner.Models;
using Oogstplanner.Services;

namespace Oogstplanner.Tests.Services
{
    [TestFixture]
    public class AuthenticationServiceTest
    {
        [Test]
        public void Services_Authentication_GetStatusAuthenticated()
        {
            // ARRANGE
            var service = new AuthenticationService();

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.IsAuthenticated)
                .Returns(true);
            Thread.CurrentPrincipal = mockPrincipal.Object;
           
            // ACT
            var result = service.GetAuthenticationStatus();

            // ASSERT
            Assert.AreEqual(AuthenticatedStatus.Authenticated, result,
                "When the user is authenticated the status enum should reflect this.");
        }

        [Test]
        public void Services_Authentication_GetStatusAnonymous()
        {
            // ARRANGE
            var service = new AuthenticationService();

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.IsAuthenticated)
                .Returns(false);
            Thread.CurrentPrincipal = mockPrincipal.Object;

            // ACT
            var result = service.GetAuthenticationStatus();

            // ASSERT
            Assert.AreEqual(AuthenticatedStatus.Anonymous, result,
                "When the user is not authenticated the status enum should be set to " +
                "anonymous.");
        }
    }
}
