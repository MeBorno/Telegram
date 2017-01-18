namespace Telegram
{
    /// <summary>
    /// Update class according to the definition on https://core.telegram.org/bots/api#update </summary>
    /// <remarks>
    /// Note that fields like 'photos', 'audio' and 'document' are all included in the file variable.</remarks>
    public class Update
    {
        // Fields
        public int update_id = -1;

        // * Optional
        public Message message = null;
        public Message edited_message = null;

        /* TODO
         * Inline queries */

        public Update() { }
        public Update(JSONObject json)
        {
            if (json == null) return;

            update_id = (int)json["update_id"];
            message = new Message((JSONObject)json["message"]);
            edited_message = new Message((JSONObject)json["edited_message"]);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} *{2}", update_id, message, edited_message);
        }
    }
}
