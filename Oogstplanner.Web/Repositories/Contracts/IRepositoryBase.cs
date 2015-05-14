namespace Oogstplanner.Repositories
{
    public interface IRepositoryBase
    {
        void Update(params object[] entities);

        void SaveChanges();
    }
}
