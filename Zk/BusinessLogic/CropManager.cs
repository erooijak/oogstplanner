using System.Collections.Generic;

using Zk.Models;
using Zk.Repositories;

namespace Zk.BusinessLogic
{
    public class CropManager
    {
        readonly Repository _repository;

        public CropManager(IZkContext db)
        {
            _repository = new Repository(db);
        }

        public IEnumerable<Crop> GetAllCrops()
        {
            return _repository.GetAllCrops();
        }

        public Crop GetCrop(int id)
        {
            return _repository.GetCrop(id);
        }

    }
}