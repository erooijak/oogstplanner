using NUnit.Framework;

using Zk.BusinessLogic;
using Zk.Models;
using Zk.Repositories;
using Zk.Tests.Fakes;
using System.Collections.Generic;

namespace Zk.Tests
{
	[TestFixture]
	public class FarmingActionManagerTest
	{
    
        FarmingActionManager _manager;
        IZkContext _db;
    
        [TestFixtureSetUp]
        public void Setup()
        {
            var calendar = new Calendar { CalendarId = 1 };
            var broccoli = new Crop 
            {
                Id = 1,
                Name = "Broccoli", 
                GrowingTime = 4,
                SowingMonths = Month.Mei ^ Month.Juni ^ Month.Oktober ^ Month.November 
            };
            // Initialize a fake database with some crops and farming actions.
            _db = new FakeZkContext 
            {
                Crops = 
                {
                    broccoli
                },
                FarmingActions = 
                {
                    new FarmingAction 
                    {
                        Id = 1,
                        Calendar = calendar,
                        Crop = broccoli,
                        Action = ActionType.Sowing,
                        CropCount = 3,
                        Month = Month.Mei
                    },
                    new FarmingAction 
                    {
                        Id = 2,
                        Calendar = calendar,
                        Crop = broccoli,
                        Action = ActionType.Harvesting,
                        CropCount = 3,
                        Month = Month.September
                    }
                }
            };

            _manager = new FarmingActionManager(_db);
        }

        [Test]
        public void FarmingActionManager_UpdateCropCounts_CorrectCropsAreUpdated()
        {
			// Arrange
            var cropIds = new List<int> { 1 };
            var cropCounts = new List<int> { 1 };

			// Act
            _manager.UpdateCropCounts(cropIds, cropCounts);

			// Assert
            Assert.AreEqual(1, _db.FarmingActions.Find(1).CropCount,
                "CropCount should be updated to 1 since the crop id with one has a count of one.");
            Assert.AreEqual(1, _db.FarmingActions.Find(2).CropCount,
                "CropCount of the related farming action should be updated to 1 too.");
        }

        [Test]
        public void FarmingActionManager_AddFarmingAction_CorrectFarmingActionsAreCreated()
        {
            // Arrange
            var action = new FarmingAction 
            {
                Action = ActionType.Harvesting,
                Calendar = new Calendar { CalendarId = 5 },
                Crop = new Crop { Id = 3, GrowingTime = 3 },
                Month = Month.April,
                CropCount = 10,
                Id = 3
            };

            // Act
            _manager.AddFarmingAction(action);

            // Assert
            var addedFarmingAction = _db.FarmingActions.Find(3); // 3 is ID specified above
            var relatedAddedFarmingAction = _db.FarmingActions.Find(0); //0 is default ID

            Assert.AreEqual(Month.April, addedFarmingAction.Month, 
                "One farming action with month january should be created");
            Assert.AreEqual(Month.Januari, relatedAddedFarmingAction.Month, 
                "A next related farming action with month April should be created");
            Assert.AreEqual(addedFarmingAction.CropCount, relatedAddedFarmingAction.CropCount,
                "The farming actions should have the same crop count.");
            Assert.AreEqual(addedFarmingAction.Calendar.CalendarId, relatedAddedFarmingAction.Calendar.CalendarId, 
                "The farming actions should have the same calendar id.");
            Assert.AreEqual(ActionType.Sowing, relatedAddedFarmingAction.Action, 
                "The related added farming action should have action type sowing (opposite of added one).");
                
        }

    }
}