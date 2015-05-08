using System.Collections.Generic;

using Oogstplanner.Models;
using Oogstplanner.Repositories;

namespace Oogstplanner.Services
{
    public class CropProvider : ICropProvider
    {
        readonly CropRepository repository;

        public CropProvider(CropRepository repository)
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