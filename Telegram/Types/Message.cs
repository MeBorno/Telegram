using System;

namespace Telegram
{
    /// <summary>
    /// Message class according to the definition on https://core.telegram.org/bots/api#message </summary>
    /// <remarks>
    /// Note that fields like 'photos', 'audio' and 'document' are all included in the file variable.</remarks>
    public class Message
    {
        // Fields       
        public int message_id = -1;
        public int date = 0;
        public Chat chat = null;

        // * Optional
        public User from = null;
        public User forward_from = null;
        public Chat forward_from_chat = null;
        public int forward_date = 0;
        public Message reply_to_message = null;
        public int edit_date = 0;
        public string text = "";
        public File file = null;

        /* TODO 
         * include all message content types */

        public Message() { }
        public Message(JSONObject json)
        {
            if (json == null) return;

            message_id = (int)json["message_id"];
            from = new User((JSONObject)json["from"]);
            date = (int)json["date"];
            chat = new Chat((JSONObject)json["chat"]);
            forward_from = new User((JSONObject)json["forward_from"]);
            forward_from_chat = new Chat((JSONObject)json["forward_from_chat"]);
            forward_date = (int)(json["forward_date"] ?? 0);
            reply_to_message = new Message((JSONObject)json["reply_to_message"]);
            edit_date = (int)(json["edit_date"] ?? 0);
            text = (string)(json["text"] ?? "");

            if (json["photo"] != null) file = new Photo((JSONArray)json["photo"]);

            Console.WriteLine(((object)(0)).GetType());
        }

        /// <summary>
        /// Checks if a message contains a file.
        /// </summary>
        /// <remarks>
        /// This overshadows all other file types.
        /// </remarks>
        public bool isFile() { return file != null; }

        /// <summary>
        /// Checks if a message contains a photo/picture.
        /// </summary>
        public bool isPhoto() { return file?.GetType() == typeof(Photo); }

        /// <summary>
        /// Checks if a message starts with a command.
        /// </summary>
        public bool isCommand() { return text?.StartsWith("/") == true; }

        /// <summary>
        /// Constructor for a response message
        /// </summary>
        /// <param name="response">text to send as a response</param>
        public Message Respond(string response)
        {
            return new Message() {
                chat = this.chat,
                text = response,
                date = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
            };
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} \"{2}\"", message_id, date, text);
        }
    }
}
