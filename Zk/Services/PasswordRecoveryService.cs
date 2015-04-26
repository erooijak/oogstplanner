using System;
using Zk.Repositories;
using System.Web.Security;

namespace Zk.Services
{
    public class PasswordRecoveryService
    {
        readonly Repository repository;

        public PasswordRecoveryService(Repository repository)
        {
            this.repository = repository;
        }

        public void StoreResetToken(string email, string token)
        {
            var timeResetRequested = DateTime.Now;
            repository.StoreResetToken(email, timeResetRequested, token);
        }

        public MembershipUser GetMembershipUserFromToken(string token)
        {
            return repository.GetMembershipUserFromToken(token);
        }

        public MembershipUser GetMembershipUserByEmail(string email)
        {
            return repository.GetMembershipUserByEmail(email);
        }

        public DateTime? GetTokenTimeStamp(string token)
        {
            return repository.GetTokenTimeStamp(token);
        }

    }
}
    