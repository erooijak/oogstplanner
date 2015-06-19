using System;
using System.Data.Entity;

using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public interface IOogstplannerContext : IDisposable
    {
        IDbSet<Crop> Crops { get; }
        IDbSet<User> Users { get; }
        IDbSet<PasswordResetToken> PasswordResetTokens { get; }
        IDbSet<Calendar> Calendars { get; }
        IDbSet<FarmingAction> FarmingActions { get; }
        IDbSet<Like> Likes { get; }
        IDbSet<T> Set<T>() where T : class;

        bool IsDeleted(object entity);
        bool IsDetached(object entity);
        void SetAdded(object entity);
        void SetDeleted(object entity);
        void SetModified(object entity);
        int SaveChanges();
    }
}
