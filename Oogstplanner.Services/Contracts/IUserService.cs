namespace Oogstplanner.Services
{
    public interface IUserService : ICommunityService
    {
        void AddUser(string userName, string fullName, string email);
        int GetCurrentUserId();
    }
}
