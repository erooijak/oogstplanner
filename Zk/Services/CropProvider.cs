using System.Collections.Generic;

using Zk.Models;
using Zk.Repositories;

namespace Zk.Services
{
    public class CropProvider
    {
        readonly Repository _repository;

        public CropProvider(Repository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Crop> GetAll()
        {
            return _repository.GetAllCrops();
        }

        public Crop Get(int id)
        {
            return _repository.GetCrop(id);
        }

    }
}