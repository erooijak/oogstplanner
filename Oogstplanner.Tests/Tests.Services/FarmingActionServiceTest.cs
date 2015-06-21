using System;
using System.Linq.Expressions;

using Moq;
using NUnit.Framework;

using Oogstplanner.Common;
using Oogstplanner.Data;
using Oogstplanner.Models;
using Oogstplanner.Services;
using Oogstplanner.Tests.Lib.Fakes;

namespace Oogstplanner.Tests.Services
{
	[TestFixture]
	public class FarmingActionServiceTest
	{       
        [Test]
        public void Services_FarmingAction_GetFarmingActions()
        {
            // ARRANGE
            var farmingActionRepositoryMock = new Mock<IFarmingActionRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            unitOfWorkMock.SetupGet(mock =>
                mock.FarmingActions).Returns(farmingActionRepositoryMock.Object);

            var fakeUserServices = new FakeUserServices();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var expectedUserId = new Random().Next();
            var expectedMonth = Months.June;

            var service = new FarmingActionService(
                unitOfWorkMock.Object, 
                fakeUserServices,
                authenticationServiceMock.Object);

            // ACT
            service.GetHarvestingActions(expectedUserId, expectedMonth);
            service.GetSowingActions(expectedUserId, expectedMonth);

            // ASSERT
            farmingActionRepositoryMock.Verify(mock => mock.GetFarmingActions
                (It.IsAny<Expression<Func<FarmingAction, bool>>>()), 
                Times.Exactly(2),
                "Calls should be delegated to repository.");
        }

        [Test]
        public void Services_FarmingAction_AddActionExisting()
        {
            // ARRANGE
            var farmingActionRepositoryMock = new Mock<IFarmingActionRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var fakeUserServices = new FakeUserServices();
            fakeUserServices.ReturnedUserService = 
                new UserService(unitOfWorkMock.Object, 
                    new Mock<ICookieProvider>().Object,
                    new Mock<ILastActivityUpdator>().Object);
                
            var expectedUserId = new Random().Next();

            var addedFarmingAction = new FarmingAction
            {
                Calendar = new Calendar { User = new User { Id = expectedUserId } },
                Month = Months.June,
                Action = ActionType.Harvesting,
                Crop = new Crop { GrowingTime = 2 },
                CropCount = 10
            };
            var returnedFarmingActions = new[] { 
                new FarmingAction{
                    Calendar = new Calendar { User = new User { Id = expectedUserId } },
                    CropCount = 10
                } 
            };

            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            farmingActionRepositoryMock.Setup(mock =>
                mock.GetFarmingActions(It.IsAny<Expression<Func<FarmingAction, bool>>>()))
                .Returns(returnedFarmingActions);
            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            unitOfWorkMock.SetupGet(mock =>
                mock.FarmingActions).Returns(farmingActionRepositoryMock.Object);

            var service = new FarmingActionService(
                unitOfWorkMock.Object, 
                fakeUserServices,
                authenticationServiceMock.Object);

            // ACT
            service.AddActionPair(addedFarmingAction);;

            // ASSERT
            farmingActionRepositoryMock.Verify(mock => mock.Update(
                It.IsAny<FarmingAction>()),
                Times.Exactly(2),
                "When an existing action is returned the crop count should" +
                "be increased once for the initial and once for the related action.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Once,
                "Changes should be committed to database.");
        }

        [Test]
        public void Services_FarmingAction_AddActionNotYetExisting()
        {
            // ARRANGE
            var farmingActionRepositoryMock = new Mock<IFarmingActionRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var fakeUserServices = new FakeUserServices();
            fakeUserServices.ReturnedUserService = 
                new UserService(unitOfWorkMock.Object, 
                    new Mock<ICookieProvider>().Object,
                    new Mock<ILastActivityUpdator>().Object);

            var expectedUserId = new Random().Next();

            var addedFarmingAction = new FarmingAction
                {
                    Calendar = new Calendar { User = new User { Id = expectedUserId } },
                    Month = Months.June,
                    Action = ActionType.Harvesting,
                    Crop = new Crop { GrowingTime = 2 },
                    CropCount = 10
                };

            var returnedFarmingActions = new[] { default(FarmingAction) };

            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            farmingActionRepositoryMock.Setup(mock =>
                mock.GetFarmingActions(It.IsAny<Expression<Func<FarmingAction, bool>>>()))
                .Returns(returnedFarmingActions);
            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            unitOfWorkMock.SetupGet(mock =>
                mock.FarmingActions).Returns(farmingActionRepositoryMock.Object);

            var service = new FarmingActionService(
                unitOfWorkMock.Object, 
                fakeUserServices,
                authenticationServiceMock.Object);

            // ACT
            service.AddActionPair(addedFarmingAction);;

            // ASSERT
            farmingActionRepositoryMock.Verify(mock => mock.Add(
                It.IsAny<FarmingAction>()),
                Times.Exactly(2),
                "A new action should be added twice: " +
                "once for the initial and once for the related action.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Once,
                "Changes should be committed to database.");
        }

        [Test]
        public void Services_FarmingAction_AddAction_UsersCannotUpdateOthers()
        {
            // ARRANGE
            var farmingActionRepositoryMock = new Mock<IFarmingActionRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var fakeUserServices = new FakeUserServices();
            fakeUserServices.ReturnedUserService = 
                new UserService(unitOfWorkMock.Object, 
                    new Mock<ICookieProvider>().Object,
                    new Mock<ILastActivityUpdator>().Object);

            const int expectedUserId = 1;
            const int anotherUserId = 2;

            var addedFarmingAction = new FarmingAction
                {
                    Calendar = new Calendar { User = new User { Id = expectedUserId } },
                    Month = Months.June,
                    Action = ActionType.Harvesting,
                    Crop = new Crop { GrowingTime = 2 },
                    CropCount = 10
                };
            var returnedFarmingActions = new[] { 
                new FarmingAction{
                    Calendar = new Calendar { User = new User { Id = expectedUserId } },
                    CropCount = 10
                } 
            };

            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            farmingActionRepositoryMock.Setup(mock =>
                mock.GetFarmingActions(It.IsAny<Expression<Func<FarmingAction, bool>>>()))
                .Returns(returnedFarmingActions);
            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(anotherUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            unitOfWorkMock.SetupGet(mock =>
                mock.FarmingActions).Returns(farmingActionRepositoryMock.Object);

            var service = new FarmingActionService(
                unitOfWorkMock.Object, 
                fakeUserServices,
                authenticationServiceMock.Object);

            // ACT
            // ASSERT
            Assert.Throws<SecurityException>(() => service.AddActionPair(addedFarmingAction),
                "When a user tries to update another user's action a security exception" +
                "should be thrown.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Never,
                "Changes should not be committed to database.");
        }

        [Test]
        public void Services_FarmingAction_RemoveAction()
        {
            // ARRANGE
            var farmingActionRepositoryMock = new Mock<IFarmingActionRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var fakeUserServices = new FakeUserServices();
            fakeUserServices.ReturnedUserService = 
                new UserService(unitOfWorkMock.Object, 
                    new Mock<ICookieProvider>().Object,
                    new Mock<ILastActivityUpdator>().Object);

            var expectedUserId = new Random().Next();

            var returnedAction = new FarmingAction
            {
                Calendar = new Calendar { User = new User { Id = expectedUserId } },
                CropCount = 10
            };
            var returnedRelatedAction = new FarmingAction
            {
                Calendar = new Calendar { User = new User { Id = expectedUserId } },
                CropCount = 10
            };

            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            const int expectedFarmingActionId = 1;

            farmingActionRepositoryMock.Setup(mock =>
                mock.GetById(expectedFarmingActionId))
                .Returns(returnedAction);
            farmingActionRepositoryMock.Setup(mock =>
                mock.FindRelated(returnedAction))
                .Returns(returnedRelatedAction);
            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            unitOfWorkMock.SetupGet(mock =>
                mock.FarmingActions).Returns(farmingActionRepositoryMock.Object);

            var service = new FarmingActionService(
                unitOfWorkMock.Object, 
                fakeUserServices,
                authenticationServiceMock.Object);

            // ACT
            service.RemoveActionPair(expectedFarmingActionId);;

            // ASSERT
            farmingActionRepositoryMock.Verify(mock => mock.Delete(returnedAction),
                Times.Once,
                "The action with the specific id should be deleted.");
            farmingActionRepositoryMock.Verify(mock => mock.Delete(returnedAction),
                Times.Once,
                "The related action should be deleted.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Once,
                "Changes should be committed to database.");
        }

        [Test]
        public void Services_FarmingAction_RemoveActionUsersCannotRemoveOthers()
        {
            // ARRANGE
            var farmingActionRepositoryMock = new Mock<IFarmingActionRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var fakeUserServices = new FakeUserServices();
            fakeUserServices.ReturnedUserService = 
                new UserService(unitOfWorkMock.Object, 
                    new Mock<ICookieProvider>().Object,
                    new Mock<ILastActivityUpdator>().Object);

            const int expectedUserId = 1;
            const int anotherUserId = 2;

            var returnedAction = new FarmingAction
                {
                    Calendar = new Calendar { User = new User { Id = expectedUserId } },
                    CropCount = 10
                };
            var returnedRelatedAction = new FarmingAction
                {
                    Calendar = new Calendar { User = new User { Id = expectedUserId } },
                    CropCount = 10
                };

            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            const int expectedFarmingActionId = 1;

            farmingActionRepositoryMock.Setup(mock =>
                mock.GetById(expectedFarmingActionId))
                .Returns(returnedAction);
            farmingActionRepositoryMock.Setup(mock =>
                mock.FindRelated(returnedAction))
                .Returns(returnedRelatedAction);
            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(anotherUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            unitOfWorkMock.SetupGet(mock =>
                mock.FarmingActions).Returns(farmingActionRepositoryMock.Object);

            var service = new FarmingActionService(
                unitOfWorkMock.Object, 
                fakeUserServices,
                authenticationServiceMock.Object);
                
            // ACT
            // ASSERT
            Assert.Throws<SecurityException>(() => service.RemoveActionPair(expectedFarmingActionId),
                "When a user tries to delete another user's action a security exception" +
                "should be thrown.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Never,
                "Changes should not be committed to database.");
        }

        [Test]
        public void Services_FarmingAction_UpdateCropCounts()
        {
            // ARRANGE
            var farmingActionRepositoryMock = new Mock<IFarmingActionRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var fakeUserServices = new FakeUserServices();
            fakeUserServices.ReturnedUserService = 
                new UserService(unitOfWorkMock.Object, 
                    new Mock<ICookieProvider>().Object,
                    new Mock<ILastActivityUpdator>().Object);

            var expectedUserId = new Random().Next();

            var returnedAction = new FarmingAction
                {
                    Calendar = new Calendar { User = new User { Id = expectedUserId } },
                    CropCount = 10
                };
            var returnedRelatedAction = new FarmingAction
                {
                    Calendar = new Calendar { User = new User { Id = expectedUserId } },
                    CropCount = 10
                };

            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            const int expectedCount = 2;
            const int expectedFarmingActionId = 1;

            farmingActionRepositoryMock.Setup(mock =>
                mock.GetById(expectedFarmingActionId))
                .Returns(returnedAction);
            farmingActionRepositoryMock.Setup(mock =>
                mock.FindRelated(returnedAction))
                .Returns(returnedRelatedAction);
            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            unitOfWorkMock.SetupGet(mock =>
                mock.FarmingActions).Returns(farmingActionRepositoryMock.Object);

            var service = new FarmingActionService(
                unitOfWorkMock.Object, 
                fakeUserServices,
                authenticationServiceMock.Object);

            var expectedIds = new[] { expectedFarmingActionId };
            var expectedCounts = new[] { expectedCount };

            // ACT
            service.UpdateCropCounts(expectedIds, expectedCounts);

            // ASSERT
            farmingActionRepositoryMock.Verify(mock => mock.Update(
                It.Is<FarmingAction>(
                    fa => fa.CropCount == expectedCount)),
                Times.Exactly(2),
                "The returned actions crop count should be updated once for original" +
                "and once for related action.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Once,
                "Changes should be committed to database.");
        }

        [Test]
        public void Services_FarmingAction_UpdateCropCountsNoChange()
        {
            // ARRANGE
            var farmingActionRepositoryMock = new Mock<IFarmingActionRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var fakeUserServices = new FakeUserServices();
            fakeUserServices.ReturnedUserService = 
                new UserService(unitOfWorkMock.Object, 
                    new Mock<ICookieProvider>().Object,
                    new Mock<ILastActivityUpdator>().Object);

            var expectedUserId = new Random().Next();

            const int currentCount = 2;
            const int expectedCount = currentCount;
            const int expectedFarmingActionId = 1;

            var returnedAction = new FarmingAction
                {
                    Calendar = new Calendar { User = new User { Id = expectedUserId } },
                    CropCount = currentCount
                };
            var returnedRelatedAction = new FarmingAction
                {
                    Calendar = new Calendar { User = new User { Id = expectedUserId } },
                    CropCount = currentCount
                };

            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            farmingActionRepositoryMock.Setup(mock =>
                mock.GetById(expectedFarmingActionId))
                .Returns(returnedAction);
            farmingActionRepositoryMock.Setup(mock =>
                mock.FindRelated(returnedAction))
                .Returns(returnedRelatedAction);
            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            unitOfWorkMock.SetupGet(mock =>
                mock.FarmingActions).Returns(farmingActionRepositoryMock.Object);

            var service = new FarmingActionService(
                unitOfWorkMock.Object, 
                fakeUserServices,
                authenticationServiceMock.Object);

            var expectedIds = new[] { expectedFarmingActionId };
            var expectedCounts = new[] { expectedCount };

            // ACT
            service.UpdateCropCounts(expectedIds, expectedCounts);

            // ASSERT
            farmingActionRepositoryMock.Verify(mock => mock.Update(It.IsAny<FarmingAction>()),
                Times.Never,
                "The repository should not be called since crop counts is same as it was.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Once,
                "No changes should be committed to database.");
        }

        [Test]
        public void Services_FarmingAction_UpdateCropCountsUserCannotUpdateOthers()
        {
            // ARRANGE
            var farmingActionRepositoryMock = new Mock<IFarmingActionRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();

            var fakeUserServices = new FakeUserServices();
            fakeUserServices.ReturnedUserService = 
                new UserService(unitOfWorkMock.Object, 
                    new Mock<ICookieProvider>().Object,
                    new Mock<ILastActivityUpdator>().Object);

            const int expectedUserId = 1;
            const int anotherUserId = 2;

            var returnedAction = new FarmingAction
                {
                    Calendar = new Calendar { User = new User { Id = expectedUserId } },
                    CropCount = 10
                };
            var returnedRelatedAction = new FarmingAction
                {
                    Calendar = new Calendar { User = new User { Id = expectedUserId } },
                    CropCount = 10
                };

            var userServiceMock = new Mock<IUserService>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            const int expectedCount = 2;
            const int expectedFarmingActionId = 1;

            farmingActionRepositoryMock.Setup(mock =>
                mock.GetById(expectedFarmingActionId))
                .Returns(returnedAction);
            farmingActionRepositoryMock.Setup(mock =>
                mock.FindRelated(returnedAction))
                .Returns(returnedRelatedAction);

            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(anotherUserId);

            fakeUserServices.ReturnedUserService = userServiceMock.Object;

            unitOfWorkMock.SetupGet(mock =>
                mock.FarmingActions).Returns(farmingActionRepositoryMock.Object);

            var service = new FarmingActionService(
                unitOfWorkMock.Object, 
                fakeUserServices,
                authenticationServiceMock.Object);

            var expectedIds = new[] { expectedFarmingActionId };
            var expectedCounts = new[] { expectedCount };

            // ACT
            // ASSERT
            Assert.Throws<SecurityException>(() => 
                service.UpdateCropCounts(expectedIds, expectedCounts),
                "When a user tries to update another user's action a security exception" +
                "should be thrown.");
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Never,
                "No changes should be committed to database.");
        }
    }
}
