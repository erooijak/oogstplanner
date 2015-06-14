using System.Linq;

using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public class PasswordRecoveryRepository 
        : EntityFrameworkRepository<PasswordResetToken>, IPasswordRecoveryRepository
    {
        public PasswordRecoveryRepository(IOogstplannerContext db) 
            : base(db)
        { }

        public PasswordResetToken GetByToken(string token)
        {
            var tokenInfo = DbSet.FirstOrDefault(prt => prt.Token == token);

            return tokenInfo;
        }
    }
}
