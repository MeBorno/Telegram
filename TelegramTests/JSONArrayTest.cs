using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telegram;

namespace TelegramTests
{
    [TestClass]
    public class JSONArrayTest
    {
        [TestMethod]
        public void EmptyArray()
        {
            JSON json = new JSON("{\"result\":[]}");
            Assert.AreEqual(((JSONArray)json["result"]).Count, 0);
        }

        [TestMethod]
        public void JSONArray()
        {
            JSON json = new JSON("{\"employees\":[{\"firstName\":\"John\",\"lastName\":\"Doe\"},{\"firstName\":\"Anna\",\"lastName\":\"Smith\"},{\"firstName\":\"Peter\",\"lastName\":\"Jones\"}]}");

            Assert.AreEqual(((JSONArray)json["employees"]).Count, 3);
            Assert.AreEqual((string)json["employees.0.firstName"], "John");
            Assert.AreEqual((string)json["employees.0.lastName"], "Doe");
            Assert.AreEqual((string)json["employees.1.firstName"], "Anna");
            Assert.AreEqual((string)json["employees.1.lastName"], "Smith");
            Assert.AreEqual((string)json["employees.2.firstName"], "Peter");
            Assert.AreEqual((string)json["employees.2.lastName"], "Jones");
        }

        [TestMethod]
        public void StressTest()
        {
            string jsonstr = "{\"result\":[{\"index\":0}";
            for (int i = 1; i < 1000; i++)
                jsonstr += ",{\"index\":"+i+"}";
            jsonstr += "]}";

            JSON json = new JSON(jsonstr);
            Assert.AreEqual(((JSONArray)json["result"]).Count, 1000);
            for (int i = 0; i < 1000; i++)
                Assert.AreEqual((int)json["result." + i + ".index"], i);
        }
    }
}
