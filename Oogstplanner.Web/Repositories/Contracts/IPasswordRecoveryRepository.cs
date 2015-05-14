using System;
using System.Web.Security;

namespace Oogstplanner.Repositories
{
    public interface IPasswordRecoveryRepository : IRepositoryBase
    {
        MembershipUser GetMembershipUserByEmail(string email);

        void StoreResetToken(string email, DateTime timeResetRequested, string token);

        MembershipUser GetMembershipUserFromToken(string token);

        DateTime? GetTokenTimeStamp(string token);
    }
}
    