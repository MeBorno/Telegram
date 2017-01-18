namespace Telegram
{
    /// <summary>
    /// PhotoSize class according to the definition on https://core.telegram.org/bots/api#photosize </summary>
    /// <remarks>
    public class PhotoSize
    {
        // Fields
        public string file_id = "";
        public int width = 0;
        public int height = 0;
        public int file_size = 0;

        public PhotoSize() { }
        public PhotoSize(JSONObject json)
        {
            if (json == null) return;

            file_id = (string)json["file_id"];
            width = (int)json["width"];
            height = (int)json["height"];
            file_size = (int)json["file_size"];
        }
    }
}
