using System;
using System.IO;
using System.Web;

using NUnit.Framework;

using Oogstplanner.Services;

namespace Oogstplanner.Tests.Services
{
    [TestFixture]
    public class CookieProviderTest
    {
        // Note: for some reason the testfixturesetup attribute does not work for this
        //       particular arrangement.
        static void Arrange()
        {
            // ARRANGE
            var httpRequest = new HttpRequest("", "www.oogstplanner.nl", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            HttpContext.Current = httpContext;
        }

        [Test]
        public void Services_Cookie_SetCookie()
        {
            // ARRANGE
            Arrange();
            const string expectedKey = "Key";
            const string expectedValue = "Value";
            const double expectedExpiration = 12d;

            var service = new CookieProvider();

            // ACT
            service.SetCookie(expectedKey, expectedValue, expectedExpiration);
            var cookie = HttpContext.Current.Response.Cookies.Get(0);

            // ASSERT
            Assert.IsTrue(HttpContext.Current.Response.Cookies.Count == 1,
                "One cookie should be set");
                
            // Note: this test might fail around 00:00.
            Assert.AreEqual(cookie.Expires.Date, DateTime.Now.AddDays(expectedExpiration).Date,
                "Expiration should be set to the one passed in");
            Assert.AreEqual(expectedKey, cookie.Name, 
                "Key should be set to the one passed in");
            Assert.AreEqual(expectedValue, cookie.Value,
                "Value should be set to the one passed in");
        }

        [Test]
        public void Services_Cookie_GetCookie()
        {
            // ARRANGE
            Arrange();
            var service = new CookieProvider();

            const string expectedKey = "Key";
            const string expectedValue = "Value";
            var cookie = new HttpCookie(expectedKey, expectedValue);

            HttpContext.Current.Request.Cookies.Add(cookie);

            // ACT
            var cookieValue = service.GetCookie(expectedKey);

            // ASSERT
            Assert.AreEqual(expectedValue, cookieValue,
                "Value of retrieved cookie should be equal to the one set.");
        }

        [Test]
        public void Services_Cookie_RemoveCookie()
        {
            // ARRANGE
            Arrange();
            var service = new CookieProvider();

            const string expectedKey = "Key";
            const string expectedValue = "Value";
            var cookie = new HttpCookie(expectedKey, expectedValue);

            HttpContext.Current.Request.Cookies.Add(cookie);

            // ACT
            service.RemoveCookie(expectedKey);

            var resultingCookie = HttpContext.Current.Response.Cookies.Get(0);

            // ASSERT
            Assert.IsTrue(resultingCookie.Expires < DateTime.Now,
                "Cookie should be set to expired.");
        }
    }
}
