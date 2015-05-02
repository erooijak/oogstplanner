﻿using NUnit.Framework;
using System;

using Oogstplanner.Utilities.ExtensionMethods;

namespace Oogstplanner.Tests
{
    [TestFixture]
    public class MonthExtensionTest
    {

        [Test]
        public void Month_Subtract()
        {
            // ACT
            var expected = Month.July;
            var actual = Month.December.Subtract(5);

            // ASSERT
            Assert.AreEqual(expected, actual, "December minus five months should equal July.");
        }

        [Test]
        public void Month_Add()
        {
            // ACT
            var expected = Month.June;
            var actual = Month.January.Add(5);

            // ASSERT
            Assert.AreEqual(expected, actual, "January plus five months should equal June.");
        }

        [Test]
        public void Month_Add_2()
        {
            // ACT
            var expected = Month.June;
            var actual = Month.May.Add(1);

            // ASSERT
            Assert.AreEqual(expected, actual, "May plus one month should equal June.");
        }

        [Test]
        public void Month_Subtract_OverTheEdge()
        {
            // ACT
            var expected = Month.July;
            var actual = Month.January.Subtract(6);

            // ASSERT
            Assert.AreEqual(expected, actual, "January minus six months should equal July.");
        }

        [Test]
        public void Month_Add_OverTheEdge()
        {
            // ACT
            var expected = Month.March;
            var actual = Month.November.Add(4);

            // ASSERT
            Assert.AreEqual(expected, actual, "November plus four months should equal March.");
        }

        [Test]
        public void Month_Add_MoreThan12()
        {
            // ACT
            var expected = Month.October;
            var actual = Month.October.Add(24);

            // ASSERT
            Assert.AreEqual(expected, actual, "October plus two years (24 months) should equal October.");
        }

        [Test]
        public void Month_Subtract_MoreThan12()
        {
            // ACT
            var expected = Month.March;
            var actual = Month.February.Subtract(23);

            // ASSERT
            Assert.AreEqual(expected, actual, "February minus 23 months should equal March.");
        }

        [Test]
        public void Month_Add_Negative()
        {   
            Assert.Throws<ArgumentOutOfRangeException>(() => Month.February.Add(-23),
                "Negative input should throw an ArgumentOutOfRangeException.");
        }

        [Test]
        public void Month_Subtract_Negative()
        {   
            Assert.Throws<ArgumentOutOfRangeException>(() => Month.February.Subtract(-23),
                "Negative input should throw an ArgumentOutOfRangeException.");
        }

    }
}