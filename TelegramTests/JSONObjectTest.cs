using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telegram;

namespace TelegramTests
{
    [TestClass]
    public class JSONObjectTest
    {
        [TestMethod]
        public void EmptyObject()
        {
            JSON json = new JSON("{ }");
            Assert.AreEqual(json.Data.Data.Count, 0);
        }

        [TestMethod]
        public void Singleton()
        {
            JSON json = new JSON("{\"name\":\"test\"}");
            Assert.AreEqual((string)json["name"], "test");
            Assert.AreEqual(json.Data.Data.Count, 1);
        }

        [TestMethod]
        public void Multivalue()
        {
            JSON json = new JSON("{\"name\":\"test\", \"age\":37}");
            Assert.AreEqual((string)json["name"], "test");
            Assert.AreEqual((int)json["age"], 37);
            Assert.AreEqual(json.Data.Data.Count, 2);
        }

        [TestMethod]
        public void NestingObject()
        {
            JSON json = new JSON("{\"result\":{\"name\":\"test\"}}");
            Assert.AreEqual(json.Data.Data.Count, 1);
            Assert.AreEqual(((JSONObject)json["result"]).Data.Count, 1);
            Assert.AreEqual(((JSONObject)json["result"])["name"], "test");
            Assert.AreEqual((string)json["result.name"], "test"); // same as the above, only checks the syntactic sugar.
        }

        [TestMethod]
        public void NestingArray()
        {
            JSON json = new JSON("{\"result\":[{\"name\":\"Aaron\"}, {\"name\":\"Bert\"}, {\"name\":\"Cole\"}]}");
            Assert.AreEqual(json.Data.Data.Count, 1);
            Assert.AreEqual(((JSONArray)json["result"]).Data.Count, 3);
            Assert.AreEqual((string)json["result.0.name"], "Aaron");
            Assert.AreEqual((string)json["result.1.name"], "Bert");
            Assert.AreEqual((string)json["result.2.name"], "Cole");
        }

        [TestMethod]
        public void JSONInvalidObjectTest()
        {
            JSON json = new JSON("{\"ok\":true,\"result\":", false);

            try { json.Parse(); }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message == "JSONObject> Expected value");
            }
        }
    }
}
