using System;
using Zk.Repositories;
using System.Web.Security;

namespace Zk.Services
{
    public class PasswordRecoveryService : IPasswordRecoveryService
    {
        readonly Repository _repository;

        public PasswordRecoveryService(Repository repository)
        {
            _repository = repository;
        }

        public void StoreResetToken(string email, string token)
        {
            var timeResetRequested = DateTime.Now;
            _repository.StoreResetToken(email, timeResetRequested, token);
        }

        public MembershipUser GetMembershipUserFromToken(string token)
        {
            return _repository.GetMembershipUserFromToken(token);
        }

        public MembershipUser GetMembershipUserByEmail(string email)
        {
            return _repository.GetMembershipUserByEmail(email);
        }

        public DateTime? GetTokenTimeStamp(string token)
        {
            return _repository.GetTokenTimeStamp(token);
        }

    }
}
    