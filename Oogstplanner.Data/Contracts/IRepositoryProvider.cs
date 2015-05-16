namespace Oogstplanner.Data
{
    public interface IRepositoryProvider
    {
        IOogstplannerContext Db { get; }
        T GetRepository<T>() where T : class;
    }
}
