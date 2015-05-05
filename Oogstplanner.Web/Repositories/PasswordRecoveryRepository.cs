using System;
using System.Linq;
using System.Web.Security;

using Oogstplanner.Models;

namespace Oogstplanner.Repositories
{
    public class PasswordRecoveryRepository : RepositoryBase
    {
        public PasswordRecoveryRepository(IOogstplannerContext db) 
            : base(db)
        {
        }

        public MembershipUser GetMembershipUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            var userName = Membership.GetUserNameByEmail(email);

            return Membership.GetUser(userName);
        }

        public void StoreResetToken(string email, DateTime timeResetRequested, string token)
        {
            var passwordReset = new PasswordResetToken
                {
                    Email = email,
                    TimeStamp = timeResetRequested,
                    Token = token
                };

            db.PasswordResetTokens.Add(passwordReset);
            db.SaveChanges();
        }

        public MembershipUser GetMembershipUserFromToken(string token)
        {
            string email = null;

            var passwordResetInstance = db.PasswordResetTokens.FirstOrDefault(pr => pr.Token == token);
            if (passwordResetInstance != null)
            {
                email = passwordResetInstance.Email;
            }

            return GetMembershipUserByEmail(email);
        }

        public DateTime? GetTokenTimeStamp(string token)
        {
            var tokenInfo = db.PasswordResetTokens.FirstOrDefault(prt => prt.Token == token);

            return tokenInfo != null ? tokenInfo.TimeStamp : (DateTime?)null;
        }
    }
}
    