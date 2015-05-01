using System.Linq;
using Oogstplanner.Models;

namespace Oogstplanner.Tests
{
    public class FakeCropSet : FakeDbSet<Crop>
    {
        public override Crop Find(params object[] keyValues)
        {
            return this.SingleOrDefault(c => c.Id == (int)keyValues.Single());
        }
    }
}