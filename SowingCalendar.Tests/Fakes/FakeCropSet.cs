using System.Linq;
using SowingCalendar.Models;

namespace SowingCalendar.Tests.Fakes
{
    public class FakeCropSet : FakeDbSet<Crop>
    {
        public override Crop Find(params object[] keyValues)
        {
            return this.SingleOrDefault(c => c.Id == (int)keyValues.Single());
        }
    }
}

