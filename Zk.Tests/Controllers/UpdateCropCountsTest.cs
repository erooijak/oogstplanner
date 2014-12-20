using NUnit.Framework;
using Zk.Models;
using Zk.Repositories;
using Zk.Tests.Fakes;
using System.Collections.Generic;

namespace Zk.Tests
{
	[TestFixture]
	public class UpdateCropCountsTest
	{
        // Unit testing the repository...
        Repository _repo;
        FakeZkContext _db;

        [TestFixtureSetUp]
        public void Setup()
        {
            // Initialize a fake database with some crops and farming actions.
            _db = new FakeZkContext {
                Crops = {
                    new Crop {
                        Id = 1,
                        Name = "Broccoli", 
                        SowingMonths = Month.Mei ^ Month.Juni ^ Month.Oktober ^ Month.November 
                    }
                },
                FarmingActions = {
                    new FarmingAction {
                        Id = 1,
                        Crop = new Crop {
                            Id = 1,
                            Name = "Broccoli", 
                            SowingMonths = Month.Mei ^ Month.Juni ^ Month.Oktober ^ Month.November 
                        },
                        CropCount = 3,
                        Month = Month.Mei
                    }
                }
            };

            _repo = new Repository(_db);
        }

		[Test]
		public void CorrectCropIsUpdated()
		{
			// Arrange
            var cropIds = new List<int> { 1 };
            var cropCounts = new List<int> { 1 };

			// Act
            _repo.UpdateCropCounts(cropIds, cropCounts);

			// Assert
            Assert.AreEqual(1, _db.FarmingActions.Find(1).CropCount,
                "CropCount should be updated to 1");
		}
	}
}