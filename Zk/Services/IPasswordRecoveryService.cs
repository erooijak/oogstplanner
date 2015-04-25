using System;
using System.Web.Security;

namespace Zk
{
    public interface IPasswordRecoveryService
    {
        void StoreResetToken(string email, string token);

        MembershipUser GetMembershipUserFromToken(string returnToken);

        DateTime? GetTokenTimeStamp(string returnToken);
    }
}