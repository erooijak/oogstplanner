using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public interface IMembershipService
    {
        bool ValidateUser(string userNameOrEmail, string password);
        void SetAuthCookie(string userNameOrEmail, bool createPersistentCookie);
        bool TryCreateUser(string username, string password, string email, out ModelError modelError);
        void AddUserToRole(string userName, string role);
        void SignOut();
    }
}
    