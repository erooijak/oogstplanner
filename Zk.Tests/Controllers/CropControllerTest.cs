using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;

using Zk.BusinessLogic;
using Zk.Controllers;
using Zk.Models;
using Zk.Repositories;

namespace Zk.Tests
{
    [TestFixture]
    public class CropControllerTest
    {
        private CropController _controller;

        [TestFixtureSetUp]
        public void Setup()
        {
            // Initialize a fake database with one crop.
            var db = new FakeZkContext
            {
                Crops =
                {
                    new Crop
                    {
                        Id = 1,
                        Name = "Broccoli", 
                        SowingMonths = Month.Mei ^ Month.Juni ^ Month.Oktober ^ Month.November 
                    }
                }
            };
            _controller = new CropController(new CropProvider(new Repository(db)));
        }

        [Test]
        public void Controllers_Crop_Index_AllCrops()
        {
            // Arrange
            var expectedResult = "Broccoli";

            // Act
            var viewResult = _controller.Index();
            var actualResult = ((IEnumerable<Crop>)viewResult.ViewData.Model).Single().Name;

            // Assert
            Assert.AreEqual(expectedResult, actualResult,
                "Since there is only a broccoli in the database the index method should return it.");
        }
    	
    }
}