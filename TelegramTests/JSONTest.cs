using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telegram;

namespace TelegramTests
{
    [TestClass]
    public class JSONTest
    {
        [TestMethod]
        public void JSONValidObjectTest()
        {
            JSON json = new JSON("{\"ok\":true,\"result\":{\"id\":164266520,\"first_name\":\"Rumble\",\"username\":\"SampRumbleBot\"}}");

            Assert.IsTrue((bool)json["ok"]);
            Assert.IsTrue((int)json["result.id"] == 164266520);
            Assert.IsTrue((string)json["result.first_name"] == "Rumble");
            Assert.IsTrue((string)json["result.username"] == "SampRumbleBot");
        }
    }
}
