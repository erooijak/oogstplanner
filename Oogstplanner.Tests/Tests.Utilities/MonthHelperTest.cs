using System.Linq;

using NUnit.Framework;

using Oogstplanner.Common;

namespace Oogstplanner.Tests.Utilities
{
    [TestFixture]
    public class MonthHelperTest
    {

        [Test]
        public void Utilities_MonthHelper_GetAllMonths()
        {
            // ACT
            var months = MonthHelper.GetAllMonths();

            // ASSERT
            Assert.AreEqual(12, months.ToList().Count,
                "Twelve months should be returned, skipping the first NotSet element.");
        }
    }
}
