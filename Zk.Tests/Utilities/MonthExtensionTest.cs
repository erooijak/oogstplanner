using NUnit.Framework;
using System;

using Zk.Utilities.ExtensionMethods;

namespace Zk.Tests
{
    [TestFixture]
    public class MonthExtensionTest
    {

        [Test]
        public void Month_Subtract()
        {
            // ACT
            var expected = Month.Juli;
            var actual = Month.December.Subtract(5);

            // ASSERT
            Assert.AreEqual(expected, actual, "December minus five months should equal July.");
        }

        [Test]
        public void Month_Add()
        {
            // ACT
            var expected = Month.Juni;
            var actual = Month.Januari.Add(5);

            // ASSERT
            Assert.AreEqual(expected, actual, "January plus five months should equal June.");
        }

        [Test]
        public void Month_Add_2()
        {
            // ACT
            var expected = Month.Juni;
            var actual = Month.Mei.Add(1);

            // ASSERT
            Assert.AreEqual(expected, actual, "May plus one month should equal June.");
        }

        [Test]
        public void Month_Subtract_OverTheEdge()
        {
            // ACT
            var expected = Month.Juli;
            var actual = Month.Januari.Subtract(6);

            // ASSERT
            Assert.AreEqual(expected, actual, "January minus six months should equal July.");
        }

        [Test]
        public void Month_Add_OverTheEdge()
        {
            // ACT
            var expected = Month.Maart;
            var actual = Month.November.Add(4);

            // ASSERT
            Assert.AreEqual(expected, actual, "November plus four months should equal March.");
        }

        [Test]
        public void Month_Add_MoreThan12()
        {
            // ACT
            var expected = Month.Oktober;
            var actual = Month.Oktober.Add(24);

            // ASSERT
            Assert.AreEqual(expected, actual, "October plus two years (24 months) should equal October.");
        }

        [Test]
        public void Month_Subtract_MoreThan12()
        {
            // ACT
            var expected = Month.Maart;
            var actual = Month.Februari.Subtract(23);

            // ASSERT
            Assert.AreEqual(expected, actual, "February minus 23 months should equal March.");
        }

        [Test]
        public void Month_Add_Negative()
        {   
            Assert.Throws<ArgumentOutOfRangeException>(() => Month.Februari.Add(-23),
                "Negative input should throw an ArgumentOutOfRangeException.");
        }

        [Test]
        public void Month_Subtract_Negative()
        {   
            Assert.Throws<ArgumentOutOfRangeException>(() => Month.Februari.Subtract(-23),
                "Negative input should throw an ArgumentOutOfRangeException.");
        }

    }
}