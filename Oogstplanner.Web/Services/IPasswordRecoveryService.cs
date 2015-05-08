using System;
using System.Web.Security;

namespace Oogstplanner.Services
{
    public interface IPasswordRecoveryService
    {
        void StoreResetToken(string email, string token);

        MembershipUser GetMembershipUserFromToken(string token);

        MembershipUser GetMembershipUserByEmail(string email);

        DateTime? GetTokenTimeStamp(string token);

        string GenerateToken();
    }
}
    