using System;
using System.Web.Security;

using Oogstplanner.Data;
using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public class PasswordRecoveryService : ServiceBase, IPasswordRecoveryService
    {
        public PasswordRecoveryService(IOogstplannerUnitOfWork unitOfWork) 
            : base(unitOfWork)
        { }

        public void StoreResetToken(string email, string token)
        {
            var resetToken = new PasswordResetToken 
                { 
                    Email = email, 
                    TimeStamp = DateTime.Now, 
                    Token = token 
                };

            UnitOfWork.PasswordRecovery.Add(resetToken);
            UnitOfWork.Commit();
        }

        public MembershipUser GetMembershipUserFromToken(string token)
        {
            var passwordResetInstance = UnitOfWork.PasswordRecovery.GetByToken(token);
            if (passwordResetInstance == null)
            {
                return null;
            }

            string email = passwordResetInstance.Email;
            return GetMembershipUserByEmail(email);
        }

        public MembershipUser GetMembershipUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            string userName = Membership.GetUserNameByEmail(email);

            return Membership.GetUser(userName);
        }

        public DateTime? GetTokenTimeStamp(string token)
        {
            var passwordResetTokenInstance = UnitOfWork.PasswordRecovery.GetByToken(token);

            return passwordResetTokenInstance == null 
                ? default(DateTime) 
                : passwordResetTokenInstance.TimeStamp;
        }

        public string GenerateToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
