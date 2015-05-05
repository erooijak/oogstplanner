using System.Collections.Generic;
using System.Linq;

using Oogstplanner.Models;

namespace Oogstplanner.Repositories
{
    public class CropRepository : RepositoryBase
    {
        public CropRepository(IOogstplannerContext db) 
            : base(db)
        {
        }

        public Crop GetCrop(int id)
        {
            return db.Crops.Single(c => c.Id == id);
        }

        public IEnumerable<Crop> GetAllCrops()
        {
            return db.Crops;
        }
    }
}
    