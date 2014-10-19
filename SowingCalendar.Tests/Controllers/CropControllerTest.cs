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
        private CropController _controller;

        [TestFixtureSetUp]
        public void Setup()
        {
            // Initialize a fake database with one crop.
            var db = new FakeSowingCalendarContext
            {
                Crops =
                {
                    new Crop
                    {
                        Id = 1,
                        Name = "Broccoli", 
                        SowingMonths = Month.May ^ Month.June ^ Month.October ^ Month.November 
                    }
                }
            };
            _controller = new CropController(db);
        }

        [Test]
        public void Controllers_Crop_From_Id_Success()
        {
            // Arrange
            var id = 1;
            var expectedResult = "Broccoli";

            // Act
            var viewResult = _controller.Crop(id);
            var actualResult = ((Crop)viewResult.ViewData.Model).Name;

            // Assert
            Assert.AreEqual(expectedResult, actualResult,
                "Since there is a broccoli with ID 1 in the database the crop method should return it.");
        }
    }
}
