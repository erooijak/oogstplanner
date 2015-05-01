using NUnit.Framework;
using System;
using System.Linq;
using Zk.Utilities.Helpers;

namespace Zk.Tests
{
    [TestFixture]
    public class MonthHelperTest
    {

        [Test]
        public void Month_GetAllMonths()
        {
            // ACT
            var months = MonthHelper.GetAllMonths();

            // ASSERT
            Assert.AreEqual(12, months.ToList().Count,
                "Twelve months should be returned, skipping the first NotSet element.");
        }

    }
}