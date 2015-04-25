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

        public IEnumerable<Crop> GetAll()
        {
            return repository.GetAllCrops();
        }

        public Crop Get(int id)
        {
            return repository.GetCrop(id);
        }

    }
}