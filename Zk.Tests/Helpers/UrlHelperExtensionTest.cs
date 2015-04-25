using NUnit.Framework;
using System.Web.Mvc;
using System.Web.Routing;

using Zk.Helpers;

namespace Zk.Tests
{
    [Ignore]
    [TestFixture]
    public class UrlHelperExtensionTest
    {

        [Test]
        public void UrlHelper_CreatePartialViewName()
        {
            // ARRANGE
            var httpContext = MvcMockHelpers.MockHttpContext(); 
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContext, routeData);
            var urlHelper = new UrlHelper(requestContext);

            // ACT
            var expected = "~/Views/Controller/_PartialView.cshtml";
            var actual = urlHelper.View("_PartialView", "Controller");

            // ASSERT
            Assert.AreEqual(expected, actual, 
                "The UrlpHelper View extension should return the path to the partial view.");
        }
         
    }
}