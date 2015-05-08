using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Security.Principal;
using System.Web;
using NUnit.Framework;

using Oogstplanner.Services;
using Oogstplanner.Models;
using Oogstplanner.Utilities.CustomExceptions;
using Oogstplanner.Repositories;

namespace Oogstplanner.Tests
{
	[TestFixture]
	public class FarmingActionServiceTest
	{
    
        FarmingActionService service;
        IOogstplannerContext db;
    
        [TestFixtureSetUp]
        public void Setup()
        {
            const string userName = "userName";

            var calendar = new Calendar { CalendarId = 1, UserId = 1 };
            var user = new User 
            { 
                    UserId = 1, 
                    Name = userName, 
                    Email = "test@test.de", 
                    AuthenticationStatus = AuthenticatedStatus.Authenticated, 
                    FullName = "test" 
            };
            var broccoli = new Crop 
            {
                Id = 1,
                Name = "Broccoli", 
                GrowingTime = 4,
                SowingMonths = Month.May ^ Month.June ^ Month.October ^ Month.November 
            };

            // Initialize a fake database with some crops and farming actions.
            this.db = new FakeOogstplannerContext 
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
                        Month = Month.May
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
                    },
                    new FarmingAction  // Used for check if an existing one is updated
                    {
                        Action = ActionType.Harvesting,
                        Calendar = new Calendar { CalendarId = 5, UserId = 1 },
                        Crop = new Crop { Id = 5, GrowingTime = 3 },
                        Month = Month.April,
                        CropCount = 10,
                        Id = 1234
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
            Thread.CurrentPrincipal = new GenericPrincipal(
                new GenericIdentity(userName),
                new string[0]
            );

            var farmingActionRepository = new FarmingActionRepository(db);
            var userRepository = new UserRepository(db);
            var calendarRepository = new CalendarRepository(db);
            service = new FarmingActionService(
                farmingActionRepository, 
                new AuthenticationService(), 
                new FakeUserServices(userRepository, calendarRepository)
            );
        }

        [Test]
        public void Services_FarmingAction_UpdateCropCounts_CorrectCropsAreUpdated()
        {
            // ARRANGE
            var farmingActionIds = new List<int> { 1 };
            var cropCounts = new List<int> { 1 };

            // ACT
            service.UpdateCropCounts(farmingActionIds, cropCounts);

            // ASSERT
            Assert.AreEqual(1, db.FarmingActions.Find(1).CropCount,
                "CropCount should be updated to 1 since the crop id with one has a count of one.");
            Assert.AreEqual(1, db.FarmingActions.Find(2).CropCount,
                "CropCount of the related farming action should be updated to 1 too.");
        }

        [Test]
        public void Services_FarmingAction_UpdateCropCounts_UserCannotEditOthers()
        {
            // ARRANGE
            var farmingActionIds = new List<int> { 1, 10 }; 
            var cropCounts = new List<int> { 1, 10 };

            // ACT AND ASSERT
            Assert.Catch<SecurityException>( () =>  service.UpdateCropCounts(farmingActionIds, cropCounts), 
                "A security exception should be thrown when a user tries to updates an action"
                + "belonging to another user.");
        }

        [Test]
        public void Services_FarmingActionAddFarmingAction_CorrectFarmingActionsAreCreated()
        {
            // ARRANGE
            var action = new FarmingAction 
            {
                Action = ActionType.Harvesting,
                Calendar = new Calendar { CalendarId = 5, UserId = 1 },
                Crop = new Crop { Id = 3, GrowingTime = 3 },
                Month = Month.April,
                CropCount = 10,
                Id = 3
            };

            // ACT
            service.AddAction(action);

            // ASSERT
            var addedFarmingAction = db.FarmingActions.Find(3); // 3 is ID specified above
            var relatedAddedFarmingAction = db.FarmingActions.Find(0); //0 is default ID

            Assert.AreEqual(Month.April, addedFarmingAction.Month, 
                "One farming action with month january should be created");
            Assert.AreEqual(Month.January, relatedAddedFarmingAction.Month, 
                "A next related farming action with month April should be created");
            Assert.AreEqual(addedFarmingAction.CropCount, relatedAddedFarmingAction.CropCount,
                "The farming actions should have the same crop count.");
            Assert.AreEqual(addedFarmingAction.Calendar.CalendarId, relatedAddedFarmingAction.Calendar.CalendarId, 
                "The farming actions should have the same calendar id.");
            Assert.AreEqual(ActionType.Sowing, relatedAddedFarmingAction.Action, 
                "The related added farming action should have action type sowing (opposite of added one).");
                
        }

        [Test]
        public void Services_FarmingActionAddFarmingAction_CropCountIsAddedToExisting()
        {
            // ARRANGE
            const int id = 1234;
           
            // Add similar to already existing one.
            var action = new FarmingAction 
            {
                Action = ActionType.Harvesting,
                Calendar = new Calendar { CalendarId = 5, UserId = 1 },
                Crop = new Crop { Id = 5, GrowingTime = 3 },
                Month = Month.April,
                CropCount = 10
            };

            // ACT
            service.AddAction(action);

            // ASSERT
            var addedFarmingAction = db.FarmingActions.Find(id);
            Assert.AreEqual(20, addedFarmingAction.CropCount, 
                "An already existing farming action should be updated if it belongs to the" 
                + "same calendar, has the same crop, type and month");
        }

        [Test]
        public void Services_FarmingActionAddFarmingAction_UserCannotEditOthers()
        {
            // ARRANGE
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

            // ACT AND ASSERT
            Assert.Catch<SecurityException>( () => service.AddAction(action), 
                "A security exception should be thrown when a user tries to edits an action"
                + "belonging to another user.");

        }

        [Test]
        public void Services_FarmingActionRemoveFarmingAction_CorrectActionRemoved()
        {
            // ARRANGE
            const int farmingActionIdToRemove = 99;
            var resultBeforeRemovingFarmingAction = db.FarmingActions.Find(farmingActionIdToRemove);

            // ACT
            service.RemoveAction(farmingActionIdToRemove);

            // ASSERT
            var resultAfterRemovingFarmingAction = db.FarmingActions.Find(farmingActionIdToRemove);

            Assert.IsInstanceOf<FarmingAction>(resultBeforeRemovingFarmingAction,
                "Before removal action should be found.");
            Assert.IsNull(resultAfterRemovingFarmingAction, 
                "Farming action with ID 99 should no longer be available after removal.");
        }

        [Test]
        public void Services_FarmingActionRemoveFarmingAction_UserCannotEditOthers()
        {
            // ARRANGE
            const int idNotBelongingToUser = 10;

            // ACT AND ASSERT
            Assert.Catch<SecurityException>( () => service.RemoveAction(idNotBelongingToUser), 
                "A security exception should be thrown when a user tries to remove an action"
                + "belonging to another user.");

        }

    }
}