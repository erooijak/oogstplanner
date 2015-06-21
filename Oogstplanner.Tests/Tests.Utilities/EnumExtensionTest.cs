using System;
using System.Linq;

using NUnit.Framework;

using Oogstplanner.Common;

namespace Oogstplanner.Tests.Utilities
{
    [TestFixture]
    public class EnumExtensionTest
    {
        const string Description1 = "First description";
        const string Description2 = "Second description";

        // ARRANGE
        enum Test1
        { 
            [System.ComponentModel.Description(Description1)]
            One = 1, 
            [System.ComponentModel.Description(Description2)]
            Two = 2
        }

        [Flags]
        enum Test2
        { 
            [System.ComponentModel.Description(Description1)]
            One = 1, 
            [System.ComponentModel.Description(Description2)]
            Two = 2
        }

        enum Test3
        { 
            One = 1, 
            Two = 2
        }

        [Test]
        public void Utilities_EnumExtension_Description()
        {
            // ACT
            var result = Test1.One.GetDescription();

            // ASSERT
            Assert.AreEqual(Description1, result, 
                "Description should be obtained from normal enumeration.");
        }

        [Test]
        public void Utilities_EnumExtension_DescriptionFlags()
        {
            // ACT
            var result = Test2.Two.GetDescription();

            // ASSERT
            Assert.AreEqual(Description2, result, 
                "Description should be obtained from flags enumeration.");
        }

        [Test]
        public void Utilities_EnumExtension_DescriptionNoAttribute()
        {
            // ACT
            var result = Test3.One.GetDescription();

            // ASSERT
            Assert.AreEqual(Test3.One.ToString(), result, 
                "When no description attribute the method should return string value.");
        }

        [Test]
        public void Utilities_EnumExtension_GetDescriptions()
        {
            // ARRANGE
            var flags = Test2.One | Test2.Two;

            // ACT
            var result = flags.GetDescriptions();

            // ASSERT
            Assert.IsTrue(result.Contains(Description1) && result.Contains(Description2), 
                "Description attributes should be returned.");
        }
    }
}
