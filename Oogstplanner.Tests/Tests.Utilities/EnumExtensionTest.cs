using NUnit.Framework;
using System;

using Oogstplanner.Utilities.ExtensionMethods;

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

    }
}