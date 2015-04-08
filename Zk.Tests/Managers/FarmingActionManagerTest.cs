using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Web;
using NUnit.Framework;

using Zk.BusinessLogic;
using Zk.Models;

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
            const string userName = "userName";

            var calendar = new Calendar { CalendarId = 1, UserId = 1 };
            var user = new User { UserId = 1, Name = userName, Email = "test@test.de", Enabled = true, FullName = "test" };
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
                Users = 
                { 
                    user
                },
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
                    },
                    new FarmingAction
                    {
                        Id = 10,
                        Calendar = new Calendar { UserId = 3, CalendarId = 2 }, // Belonging to other user.
                        Crop = broccoli,
                        Action = ActionType.Harvesting,
                        CropCount = 5,
                        Month = Month.September
                    },
                    new FarmingAction // Used for removal check
                    {
                        Id = 99,
                        Calendar = calendar,
                        Crop = new Crop { Id = 9999, GrowingTime = 3 }, // No correlation with others
                        Action = ActionType.Harvesting,
                        CropCount = 5,
                        Month = Month.September
                    }
                }
            };
                    
            // Fake the HttpContext which is used for the check if the user is allowed to update the action.
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
            );

            // User is logged in
            HttpContext.Current.User = new GenericPrincipal(
                new GenericIdentity(userName),
                new string[0]
            );

            _manager = new FarmingActionManager(_db);
        }

        [Test]
        public void FarmingActionManager_UpdateCropCounts_CorrectCropsAreUpdated()
        {
            // Arrange
            var farmingActionIds = new List<int> { 1 };
            var cropCounts = new List<int> { 1 };

            // Act
            _manager.UpdateCropCounts(farmingActionIds, cropCounts);

            // Assert
            Assert.AreEqual(1, _db.FarmingActions.Find(1).CropCount,
                "CropCount should be updated to 1 since the crop id with one has a count of one.");
            Assert.AreEqual(1, _db.FarmingActions.Find(2).CropCount,
                "CropCount of the related farming action should be updated to 1 too.");
        }

        [Test]
        public void FarmingActionManager_UpdateCropCounts_UserCannotEditOthers()
        {
            // Arrange
            var farmingActionIds = new List<int> { 1, 10 }; 
            var cropCounts = new List<int> { 1, 10 };

            // Act and Assert
            Assert.Catch<SecurityException>( () =>  _manager.UpdateCropCounts(farmingActionIds, cropCounts), 
                "A security exception should be thrown when a user tries to updates an action"
                + "belonging to another user.");
        }

        [Test]
        public void FarmingActionManager_AddFarmingAction_CorrectFarmingActionsAreCreated()
        {
            // Arrange
            var action = new FarmingAction 
            {
                Action = ActionType.Harvesting,
                Calendar = new Calendar { CalendarId = 5, UserId = 1 },
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

        [Test]
        public void FarmingActionManager_AddFarmingAction_UserCannotEditOthers()
        {
            // Arrange
            const int differentUserIdThanReturnedByHttpContext = 3;

            var action = new FarmingAction 
            {
                Action = ActionType.Harvesting,
                Calendar = new Calendar { CalendarId = 5, UserId = differentUserIdThanReturnedByHttpContext },
                Crop = new Crop { Id = 3, GrowingTime = 3 },
                Month = Month.April,
                CropCount = 10,
                Id = 3
            };

            // Act and Assert
            Assert.Catch<SecurityException>( () => _manager.AddFarmingAction(action), 
                "A security exception should be thrown when a user tries to edits an action"
                + "belonging to another user.");

        }

        [Test]
        public void FarmingActionManager_RemoveFarmingAction_CorrectActionRemoved()
        {
            // Arrange
            const int farmingActionIdToRemove = 99;
            var resultBeforeRemovingFarmingAction = _db.FarmingActions.Find(farmingActionIdToRemove);

            // Act
            _manager.RemoveFarmingAction(farmingActionIdToRemove);

            // Assert
            var resultAfterRemovingFarmingAction = _db.FarmingActions.Find(farmingActionIdToRemove);

            Assert.IsInstanceOf<FarmingAction>(resultBeforeRemovingFarmingAction,
                "Before removal action should be found.");
            Assert.IsNull(resultAfterRemovingFarmingAction, 
                "Farming action with ID 99 should no longer be available after removal.");
        }

        [Test]
        public void FarmingActionManager_RemoveFarmingAction_UserCannotEditOthers()
        {
            // Arrange
            const int idNotBelongingToUser = 10;

            // Act and Assert
            Assert.Catch<SecurityException>( () => _manager.RemoveFarmingAction(idNotBelongingToUser), 
                "A security exception should be thrown when a user tries to remove an action"
                + "belonging to another user.");

        }

    }
}