using System.Linq;
using Oogstplanner.Models;

namespace Oogstplanner.Tests
{
    public class FakeUserSet : FakeDbSet<User>
    {
        public override User Find(params object[] keyValues)
        {
            return this.SingleOrDefault(c => c.UserId == (int)keyValues.Single());
        }
    }
}