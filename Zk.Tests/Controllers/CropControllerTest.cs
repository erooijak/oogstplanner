using System;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using Zk.Controllers;
using Zk.Models;
using Zk.Repositories;
using Zk.Tests.Fakes;

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

		[Test]
		public void Controllers_Crop_From_Name_Success()
		{
			// Arrange
			var name = "Broccoli";

			// Act
			var viewResult = _controller.Crop(name);
			var result = ((Crop)viewResult.ViewData.Model).SowingMonths;

			// Assert
			Assert.AreEqual(Month.May ^ Month.June ^ Month.October ^ Month.November, result,
				"Since there is a crop with the name broccoli in the database the crop method should return it" +
				"and the sowing months should be may, june, october and november.");
		}
	}
}