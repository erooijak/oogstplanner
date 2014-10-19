using System.Data.Entity;
using SowingCalendar.Models;
using SowingCalendar.Tests.Fakes;

namespace SowingCalendar.Tests.Fakes
{
    public class FakeSowingCalendarContext : ISowingCalendarContext
    {
        public FakeSowingCalendarContext()
        {
            Crops = new FakeCropSet();
        }

        public IDbSet<Crop> Crops { get; private set; }

        public int SaveChanges()
        {
            return 0;
        }

    }
}

