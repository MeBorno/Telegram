namespace Telegram
{
    /// <summary>
    /// User class according to the definition on https://core.telegram.org/bots/api#user </summary>
    /// <remarks>
    /// Note that fields like 'photos', 'audio' and 'document' are all included in the file variable.</remarks>
    public class User
    {
        // Fields
        public int id = -1;
        public string first_name = "";

        // * Optional
        public string last_name = "";
        public string username = "";

        public User() { }
        public User(JSONObject json)
        {
            if (json == null) return;

            id = (int)json["id"];
            first_name = (string)json["first_name"];
            last_name = (string)(json["last_name"] ?? "");
            username = (string)(json["username"] ?? "");
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} {2} {3}", id, first_name, last_name, username);
        }
    }
}
