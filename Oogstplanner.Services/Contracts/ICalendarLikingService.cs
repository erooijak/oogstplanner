using System.Collections.Generic;

using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public interface ICalendarLikingService
    {
        void Like(int calendarId);
        void UnLike(int calendarId);
        IEnumerable<Like> GetLikes(int calendarId);
    }
}
