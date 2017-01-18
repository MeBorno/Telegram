using System;

namespace Telegram
{
    // Quick helper to catch errors.
    public class JSONError : JSON
    {
        public JSONError(Exception e) : base()
        {
            Data["Ok"] = false;
            Data["Error"] = e.Message;
        }

        public override bool isError()
        {
            return true;
        }
    }
}
