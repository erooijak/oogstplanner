using System.Web.Security;

namespace Oogstplanner.Services
{
    public class OogstplannerMembershipService : IMembershipService
    {
        public bool ValidateUser(string userNameOrEmail, string password)
        {
            return Membership.ValidateUser(userNameOrEmail, password)
                || Membership.ValidateUser(Membership.GetUserNameByEmail(userNameOrEmail), password);
        }

        public void SetAuthCookie(string userNameOrEmail, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userNameOrEmail, createPersistentCookie);
        }

        public bool TryCreateUser(string userName, string password, string email, out MembershipCreateStatus status)
        {
            (Membership.Provider).CreateUser(
                userName, password, email, null, null, true, null, out status);

            return status == MembershipCreateStatus.Success;
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
    