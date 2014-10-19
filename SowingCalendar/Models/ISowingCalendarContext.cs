using System.Data.Entity;

namespace SowingCalendar.Models
{
    public interface ISowingCalendarContext
    {
        IDbSet<Crop> Crops { get; }

        int SaveChanges();
    }
}

