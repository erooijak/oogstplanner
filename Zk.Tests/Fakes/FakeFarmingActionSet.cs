using System.Linq;
using Zk.Models;

namespace Zk.Tests
{
    public class FakeCropSet : FakeDbSet<Crop>
    {
        public override Crop Find(params object[] keyValues)
        {
            return this.SingleOrDefault(c => c.Id == (int)keyValues.Single());
        }
    }
}