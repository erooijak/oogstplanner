using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Moq;
using NUnit.Framework;

using Oogstplanner.Data;
using Oogstplanner.Models;
using Oogstplanner.Services;
using Oogstplanner.Web.Controllers;

namespace Oogstplanner.Tests.Controllers
{
    [TestFixture]
    public class CropControllerTest
    {
        CropController controller;

        [TestFixtureSetUp]
        public void Setup()
        {
            var expectedCrop = new Crop
            {
                Id = 1,
                Name = "Broccoli", 
                SowingMonths = Months.May ^ Months.June ^ Months.October ^ Months.November 
            };

            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            unitOfWorkMock.Setup(mock => 
                mock.Crops.GetAll())
                .Returns(new[] { expectedCrop });

            this.controller = new CropController(new CropProvider(unitOfWorkMock.Object));
        }

        [Test]
        public void Controllers_Crop_Index_AllCrops()
        {
            // ARRANGE
            const string expectedResult = "Broccoli";

            // ACT
            var viewResult = controller.Index();
            var actualResult = ((IEnumerable<Crop>)viewResult.ViewData.Model).Single().Name;

            // ASSERT
            Assert.AreEqual(expectedResult, actualResult,
                "Since there is only a broccoli in the database the index method should return it.");
        }

        [Test]
        public void Controllers_Crop_All()
        {
            // ARRANGE
            const string expectedResult = "Broccoli";

            // ACT
            var viewResult = controller.All();
            var actualResult = ((ContentResult)viewResult).Content;

            // ASSERT
            Assert.IsTrue(actualResult.Contains(expectedResult),
                "Since there is a broccoli in the database the JSON string returned by the all " +
                "method should contain it.");
        }    	
    }
}
