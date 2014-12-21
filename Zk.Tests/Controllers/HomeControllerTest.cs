using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Zk;
using Zk.Controllers;

namespace Zk.Tests
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void Controllers_Home_Index()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = (ViewResult)controller.Index();

            const string expectedVersion = "4.0";
            const string expectedRuntime = "Mono";

            // Assert
            Assert.AreEqual(expectedVersion, result.ViewData["Version"]);
            Assert.AreEqual(expectedRuntime, result.ViewData["Runtime"]);
        }
    }
}