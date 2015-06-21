using System;

namespace Oogstplanner.Services
{
    public interface IPasswordRecoveryService
    {
        void StoreResetToken(string email, string token);
        string GetEmailFromToken(string token);
        DateTime? GetTokenTimeStamp(string token);
        string GenerateToken();
    }
}
