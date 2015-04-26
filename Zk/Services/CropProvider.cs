using System.Collections.Generic;

using Zk.Models;
using Zk.Repositories;

namespace Zk.Services
{
    public class CropProvider
    {
        readonly Repository repository;

        public CropProvider(Repository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Crop> GetAllCrops()
        {
            return repository.GetAllCrops();
        }

        public Crop GetCrop(int id)
        {
            return repository.GetCrop(id);
        }

    }
}