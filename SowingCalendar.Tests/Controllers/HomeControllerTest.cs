using System;
using System.Web.Mvc;
using NUnit.Framework;
using SowingCalendar.Controllers;
using SowingCalendar.Models;
using SowingCalendar.Repositories;
using SowingCalendar.Tests.Fakes;

namespace SowingCalendar.Tests.Controllers
{
    [TestFixture]
    public class CropControllerTest
    {
        readonly ISowingCalendarContext _db;
        readonly Repository _repo;
        private CropController _controller;

        [TestFixtureSetUp]
        public void Setup()
        {
            // Arrange
            _db = new FakeSowingCalendarContext();
            _controller = new CropController();
        }

        [Test]
        public void Controllers_Crop_Success()
        {
            // Arrange


            // Act
            var result = (ViewResult)_controller.Index();

            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            var expectedVersion = mvcName.Version.Major;
            var expectedRuntime = isMono ? "Mono" : ".NET";

            // Assert
            Assert.AreEqual(expectedVersion, result.ViewData["Version"]);
            Assert.AreEqual(expectedRuntime, result.ViewData["Runtime"]);
        }
    }
}
