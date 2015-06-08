namespace Oogstplanner.Services
{
    public interface ICalendarLikingService
    {
        void Like(int calendarId);
        void UnLike(int calendarId);
    }
}
