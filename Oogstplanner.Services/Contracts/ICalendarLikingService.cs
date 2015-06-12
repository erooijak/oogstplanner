using System.Collections.Generic;

using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public interface ICalendarLikingService
    {
        void Like(int calendarId, out bool wasUnlike);
        IEnumerable<Like> GetLikes(int calendarId);
    }
}
