using System;

using Moq;
using NUnit.Framework;

using Oogstplanner.Data;
using Oogstplanner.Services;

namespace Oogstplanner.Tests.Services
{
    [TestFixture]
    public class CropProviderTest
    {
        [Test]
        public void Services_Crop_GetAllCrops()
        {
            // ARRANGE
            var cropRepositoryMock = new Mock<ICropRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            unitOfWorkMock.SetupGet(mock => mock.Crops)
                .Returns(cropRepositoryMock.Object);

            var service = new CropProvider(unitOfWorkMock.Object);

            // ACT
            service.GetAllCrops();

            // ASSERT
            cropRepositoryMock.Verify(mock =>
                mock.GetAll(), Times.Once,
                "All crops should be retrieved from the repository.");
        }

        [Test]
        public void Services_Crop_GetById()
        {
            // ARRANGE
            var cropRepositoryMock = new Mock<ICropRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            unitOfWorkMock.SetupGet(mock => mock.Crops)
                .Returns(cropRepositoryMock.Object);

            var service = new CropProvider(unitOfWorkMock.Object);

            var expectedCropId = new Random().Next();

            // ACT
            service.GetCrop(expectedCropId);

            // ASSERT
            cropRepositoryMock.Verify(mock =>
                mock.GetById(expectedCropId), Times.Once,
                "The crop with the expected id should be obtained from " +
                "the repository.");
        }

        [Test]
        public void Services_Crop_NoCommit()
        {
            // ARRANGE
            var cropRepositoryMock = new Mock<ICropRepository>();
            var unitOfWorkMock = new Mock<IOogstplannerUnitOfWork>();
            unitOfWorkMock.SetupGet(mock => mock.Crops)
                .Returns(cropRepositoryMock.Object);

            var service = new CropProvider(unitOfWorkMock.Object);

            // ACT
            service.GetAllCrops();
            service.GetCrop(1);

            // ASSERT
            unitOfWorkMock.Verify(mock => mock.Commit(), Times.Never,
                "No changes to commit to database.");
        }
    }
}
