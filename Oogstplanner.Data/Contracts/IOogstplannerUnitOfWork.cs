namespace Oogstplanner.Data
{
    public interface IOogstplannerUnitOfWork
    {
        ICalendarRepository Calendars { get; }
        ICropRepository Crops { get; }
        IPasswordRecoveryRepository PasswordRecovery { get; }
        IFarmingActionRepository FarmingActions { get; }
        IUserRepository Users { get; }

        void Commit();
    }
}
