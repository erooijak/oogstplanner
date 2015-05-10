using System;
using System.Web.Security;

using Oogstplanner.Repositories;

namespace Oogstplanner.Services
{
    public class PasswordRecoveryService : IPasswordRecoveryService
    {
        readonly IPasswordRecoveryRepository repository;

        public PasswordRecoveryService(IPasswordRecoveryRepository repository)
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

        public string GenerateToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
    