using System.Web.Security;

using Oogstplanner.Utilities.Helpers;
using Oogstplanner.Utilities.CustomClasses;

namespace Oogstplanner.Services
{
    public class MembershipService : IMembershipService
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

        public bool TryCreateUser(string userName, string password, string email, out ModelError modelError)
        {
            modelError = new ModelError();
            MembershipCreateStatus membershipCreateStatus;
            (Membership.Provider).CreateUser(
                userName, password, email, null, null, true, null, out membershipCreateStatus);

            if (membershipCreateStatus == MembershipCreateStatus.Success)
            {
                return true;
            }
            else
            {
                modelError.Field = MembershipHelper.ErrorCodeToField(membershipCreateStatus);
                modelError.Message = MembershipHelper.ErrorCodeToString(membershipCreateStatus);

                return false;
            }
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
    