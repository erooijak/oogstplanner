using System.Web.Mvc;
using System.Web.Routing;

using NUnit.Framework;

using Oogstplanner.Tests.Lib;
using Oogstplanner.Web.Utilities.ExtensionMethods;

namespace Oogstplanner.Tests.Utilities
{

    [TestFixture]
    public class UrlHelperExtensionTest
    {

        [Test]
        public void Utilities_UrlHelper_CreatePartialViewName()
        {
            // ARRANGE
            var httpContext = MvcMockHelpers.MockHttpContext(); 
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContext, routeData);
            var urlHelper = new UrlHelper(requestContext);

            // ACT
            const string expected = "~/Views/Controller/_PartialView.cshtml";
            var actual = urlHelper.View("_PartialView", "Controller");

            // ASSERT
            Assert.AreEqual(expected, actual, 
                "The UrlpHelper View extension should return the path to the partial view.");
        }         
    }
}
