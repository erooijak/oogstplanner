namespace Oogstplanner.Services
{
    public interface IDeletableUserService : IUserService
    {
        void RemoveUser(int userId);
    }
}
