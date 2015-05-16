using System.Collections.Generic;

using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public interface ICropProvider
    {
        IEnumerable<Crop> GetAllCrops();
        Crop GetCrop(int id);
    }
}
