using System.Data.Entity;

namespace Zk.Models
{
    public interface IZkContext
    {
        IDbSet<Crop> Crops { get; }
        IDbSet<FarmingAction> FarmingActions { get; }
        IDbSet<Calendar> Calendars { get; }
        IDbSet<User> Users { get; }
        IDbSet<PasswordResetToken> PasswordResetTokens { get; }

        void SetModified(object entity);
	int SaveChanges();
    }
}