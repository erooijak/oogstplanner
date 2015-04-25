using System;
using System.Web.Security;

namespace Zk.Services
{
    public interface IPasswordRecoveryService
    {
        void StoreResetToken(string email, string token);

        MembershipUser GetMembershipUserFromToken(string returnToken);

        DateTime? GetTokenTimeStamp(string returnToken);

        MembershipUser GetMembershipUserByEmail(string email);
    }
}