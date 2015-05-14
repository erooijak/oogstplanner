using System.Collections.Generic;
using System.Linq;

using Oogstplanner.Models;

namespace Oogstplanner.Repositories
{
    public interface ICropRepository : IRepositoryBase
    {
        Crop GetCrop(int id);

        IEnumerable<Crop> GetAllCrops();
    }
}
    