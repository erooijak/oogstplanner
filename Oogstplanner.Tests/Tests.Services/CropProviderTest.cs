using Moq;
using NUnit.Framework;

using System;

using Oogstplanner.Repositories;
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

            var service = new CropProvider(cropRepositoryMock.Object);

            // ACT
            service.GetAllCrops();

            // ASSERT
            cropRepositoryMock.Verify(mock =>
                mock.GetAllCrops(), Times.Once,
                "All crops should be retrieved from the repository.");
        }

        [Test]
        public void Services_Crop_GetById()
        {
            // ARRANGE
            var cropRepositoryMock = new Mock<ICropRepository>();

            var service = new CropProvider(cropRepositoryMock.Object);

            var expectedCropId = new Random().Next();

            // ACT
            service.GetCrop(expectedCropId);

            // ASSERT
            cropRepositoryMock.Verify(mock =>
                mock.GetCrop(expectedCropId), Times.Once,
                "The crop with the expected id should be obtained from " +
                "the repository.");
        }
    }
}
