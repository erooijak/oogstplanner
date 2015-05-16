using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public class CropRepository : EntityFrameworkRepository<Crop>, ICropRepository
    {
        public CropRepository(IOogstplannerContext db) : base(db)
        {
        }           
    }
}
