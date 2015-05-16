using System;
using System.Linq;
using System.Web.Security;

using NUnit.Framework;

using Oogstplanner.Common;
using Oogstplanner.Models;

namespace Oogstplanner.Tests.Utilities
{
    [TestFixture]
    public class MembershipHelperTest
    {
        [Test]
        public void Utilities_MembershipHelper_ErrorCodeToString()
        {
            // ARRANGE
            var status = MembershipCreateStatus.DuplicateUserName;

            // ACT
            var result = MembershipHelper.ErrorCodeToString(status);

            // ASSERT
            Assert.IsTrue(result.Contains("Gebruikersnaam bestaat al"), result, 
                "Helper should return string for error code.");
        }

        [Test]
        public void Utilities_MembershipHelper_ErrorCodeToKey()
        {
            // ARRANGE
            var status = MembershipCreateStatus.DuplicateUserName;

            // ACT
            var result = MembershipHelper.ErrorCodeToField(status);

            // ASSERT
            Assert.AreEqual("UserName", result, 
                "Helper should return field belonging to error.");
        }

        [Test]
        public void Utilities_MembershipHelper_PropertiesMatch()
        {
            // ARRANGE
            var registerModel = new RegisterModel();
            string[] propertyNames = registerModel.GetType().GetProperties()
                .Select(p => p.Name).ToArray();

            string[] errorCodeKeys = Enum.GetValues(typeof(MembershipCreateStatus))
                .Cast<MembershipCreateStatus>()
                .Select(s => MembershipHelper.ErrorCodeToField(s))
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();
             
            // ASSERT
            foreach (var errorCodeKey in errorCodeKeys)
            {
                Assert.IsTrue(propertyNames.Contains(errorCodeKey),
                    "Fields belonging to error codes should match property names, " +
                    "but {0} does not exist on RegisterModel.", errorCodeKey);
            }
        }
    }
}
