using System.Collections.Generic;

using Oogstplanner.Models;
using Oogstplanner.Repositories;

namespace Oogstplanner.Services
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