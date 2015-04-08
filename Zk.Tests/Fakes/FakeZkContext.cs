using System.Data.Entity;
using Zk.Models;

namespace Zk.Tests
{
    public class FakeZkContext : IZkContext
    {
        public FakeZkContext()
        {
            Crops = new FakeCropSet();
            FarmingActions = new FakeFarmingActionSet();
            Users = new FakeUserSet();
        }

        public IDbSet<Crop> Crops { get; private set; }

        public IDbSet<FarmingAction> FarmingActions { get; private set; }

        public IDbSet<Calendar> Calendars { get; private set; }

        public IDbSet<User> Users { get; private set; }

        public IDbSet<PasswordResetToken> PasswordResetTokens { get; private set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void SetModified(object entity)
        {
        }

    }
}