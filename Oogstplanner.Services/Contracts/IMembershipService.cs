using System.Web.Security;

using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public interface IMembershipService
    {
        bool ValidateUser(string userNameOrEmail, string password);
        MembershipUser GetMembershipUserByEmail(string email);
        bool TryCreateUser(string username, string password, string email, out ModelError modelError);
        void SetAuthCookie(string userNameOrEmail, bool createPersistentCookie);
        void AddUserToRole(string userName, string role);
        void SignOut();
        void RemoveUser(string userName);
    }
}
