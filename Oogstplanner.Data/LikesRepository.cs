using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public class LikesRepository : EntityFrameworkRepository<Like>, ILikesRepository
    {
        public LikesRepository(IOogstplannerContext db) : base(db)
        {
        }
    }
}
