using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public interface IPasswordRecoveryRepository : IRepository<PasswordResetToken>
    {
        PasswordResetToken GetByToken(string token);
    }
}
  