using System;

using NUnit.Framework;

using Oogstplanner.Models;

namespace Oogstplanner.Tests.Utilities
{
    [TestFixture]
    public class MonthExtensionTest
    {

        [Test]
        public void Utilities_MonthExtension_Subtract()
        {
            // ACT
            var expected = Months.July;
            var actual = Months.December.Subtract(5);

            // ASSERT
            Assert.AreEqual(expected, actual, "December minus five months should equal July.");
        }

        [Test]
        public void Utilities_MonthExtension_Add()
        {
            // ACT
            var expected = Months.June;
            var actual = Months.January.Add(5);

            // ASSERT
            Assert.AreEqual(expected, actual, "January plus five months should equal June.");
        }

        [Test]
        public void Utilities_MonthExtension_Add_2()
        {
            // ACT
            var expected = Months.June;
            var actual = Months.May.Add(1);

            // ASSERT
            Assert.AreEqual(expected, actual, "May plus one month should equal June.");
        }

        [Test]
        public void Utilities_MonthExtension_Subtract_OverTheEdge()
        {
            // ACT
            var expected = Months.July;
            var actual = Months.January.Subtract(6);

            // ASSERT
            Assert.AreEqual(expected, actual, "January minus six months should equal July.");
        }

        [Test]
        public void Utilities_MonthExtension_Add_OverTheEdge()
        {
            // ACT
            var expected = Months.March;
            var actual = Months.November.Add(4);

            // ASSERT
            Assert.AreEqual(expected, actual, "November plus four months should equal March.");
        }

        [Test]
        public void Utilities_MonthExtension_Add_MoreThan12()
        {
            // ACT
            var expected = Months.October;
            var actual = Months.October.Add(24);

            // ASSERT
            Assert.AreEqual(expected, actual, "October plus two years (24 months) should equal October.");
        }

        [Test]
        public void Utilities_MonthExtension_Subtract_MoreThan12()
        {
            // ACT
            var expected = Months.March;
            var actual = Months.February.Subtract(23);

            // ASSERT
            Assert.AreEqual(expected, actual, "February minus 23 months should equal March.");
        }

        [Test]
        public void Utilities_MonthExtension_Add_Negative()
        {   
            Assert.Throws<ArgumentOutOfRangeException>(() => Months.February.Add(-23),
                "Negative input should throw an ArgumentOutOfRangeException.");
        }

        [Test]
        public void Utilities_MonthExtension_Subtract_Negative()
        {   
            Assert.Throws<ArgumentOutOfRangeException>(() => Months.February.Subtract(-23),
                "Negative input should throw an ArgumentOutOfRangeException.");
        }

        [Test]
        public void Utilities_MonthExtension_Add_Flags()
        {   
            Assert.Throws<ArgumentException>(() 
                => (Months.February | Months.March).Add(2),
                "When multiple months are selected the method should throw an ArgumentException.");
        }

        [Test]
        public void Utilities_MonthExtension_Subtract_Flags()
        {   
            Assert.Throws<ArgumentException>(() 
                => (Months.February | Months.December).Subtract(2),
                "When multiple months are selected the method should throw an ArgumentException.");
        }
    }
}
