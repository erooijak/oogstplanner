using System.Linq;
using Oogstplanner.Models;

namespace Oogstplanner.Tests.Lib.Fakes
{
    public class FakeFarmingActionSet : FakeDbSet<FarmingAction>
    {	
        public override FarmingAction Find(params object[] keyValues)
        {
            return this.SingleOrDefault(c => c.Id == (int)keyValues.Single());
        }
    }
}