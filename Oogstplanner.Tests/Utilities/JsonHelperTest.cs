using NUnit.Framework;
using System.Linq;
using System.Web.Mvc;

using Newtonsoft.Json;

using Oogstplanner.Utitilies.Helpers;

namespace Oogstplanner.Tests
{
    [TestFixture]
    public class JsonHelperTest
    {
        [Test]
        public void JsonHelper_CreateErrorModel()
        {
            // ARRANGE
            const string EXPECTED_KEY1 = "test1";
            const string EXPECTED_KEY2 = "test2";
            const string EXPECTED_ERROR1= "An error";
            const string EXPECTED_ERROR2 = "A second error";

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(EXPECTED_KEY1, EXPECTED_ERROR1);
            modelState.AddModelError(EXPECTED_KEY2, EXPECTED_ERROR2);

            // ACT
            var actual = JsonHelper.CreateErrorModel(modelState);

            // ASSERT
            Assert.IsTrue(actual.Count() == 2, "2 values should be added to the JSON representation.");
            Assert.AreEqual("[{\"key\":\"" + EXPECTED_KEY1 + "\",\"errors\":[\"" + EXPECTED_ERROR1 + 
                "\"]},{\"key\":\"" + EXPECTED_KEY2 + "\",\"errors\":[\"" + EXPECTED_ERROR2 + "\"]}]",
                JsonConvert.SerializeObject(actual), "A JSON string with the keys and errors should be obtained");
        }
    }
}