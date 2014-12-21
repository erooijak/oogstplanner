using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;

using Zk.Controllers;
using Zk.BusinessLogic;
using Zk.Models;
using Zk.Tests.Fakes;
using Zk.Repositories;

namespace Zk.Tests.Controllers
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
            var repository = new Repository(db);
            var manager = new CropManager(repository);
            _controller = new CropController(manager);
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