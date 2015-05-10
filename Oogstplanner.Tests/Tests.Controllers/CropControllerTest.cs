using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;

using Oogstplanner.Tests.Lib.Fakes;
using Oogstplanner.Services;
using Oogstplanner.Controllers;
using Oogstplanner.Models;
using Oogstplanner.Repositories;

namespace Oogstplanner.Tests.Controllers
{
    [TestFixture]
    public class CropControllerTest
    {
        private CropController controller;

        [TestFixtureSetUp]
        public void Setup()
        {
            // Initialize a fake database with one crop.
            var db = new FakeOogstplannerContext
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
            this.controller = new CropController(new CropProvider(new CropRepository(db)));
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