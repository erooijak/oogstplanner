using System.Data.Entity;
using Oogstplanner.Models;

namespace Oogstplanner.Tests.Lib.Fakes
{
    public class FakeOogstplannerContext : IOogstplannerContext
    {
        public FakeOogstplannerContext()
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