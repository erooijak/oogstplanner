using System.Collections.Generic;

using Oogstplanner.Data;
using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public class CropProvider : ServiceBase, ICropProvider
    {
        public CropProvider(IOogstplannerUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IEnumerable<Crop> GetAllCrops()
        {
            return UnitOfWork.Crops.GetAll();
        }

        public Crop GetCrop(int id)
        {
            return UnitOfWork.Crops.GetById(id);
        }

    }
}
