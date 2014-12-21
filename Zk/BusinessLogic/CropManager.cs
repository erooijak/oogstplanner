using System.Collections.Generic;

using Zk.Models;
using Zk.Repositories;

namespace Zk.BusinessLogic
{
    public class CropManager
    {
        readonly Repository _repository;

        public CropManager()
        {
            _repository = new Repository();
        }

        public CropManager(Repository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Crop> GetAllCrops()
        {
            return _repository.GetAllCrops();
        }
            
    }
}