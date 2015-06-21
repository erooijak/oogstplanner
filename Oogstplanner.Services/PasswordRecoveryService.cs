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

        public string GetEmailFromToken(string token)
        {
            var passwordResetInstance = UnitOfWork.PasswordRecovery.GetByToken(token);
            if (passwordResetInstance == null)
            {
                return null;
            }

            return passwordResetInstance.Email;
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
