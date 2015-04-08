using System.Linq;
using Zk.Models;

namespace Zk.Tests
{
    public class FakeUserSet : FakeDbSet<User>
    {
        public override User Find(params object[] keyValues)
        {
            return this.SingleOrDefault(c => c.UserId == (int)keyValues.Single());
        }
    }
}