namespace Telegram
{
    /// <summary>
    /// Chat class according to the definition on https://core.telegram.org/bots/api#chat </summary>
    public class Chat
    {
        // Fields
        public int id = -1;
        public string type = "";
        
        // * Optional
        public string title = "";
        public string username = "";
        public string first_name = "";
        public string last_name = "";
        public bool all_members_are_administrators = false;

        public Chat() { }
        public Chat(JSONObject json)
        {
            if (json == null) return;

            id = (int)json["id"];
            type = (string)json["type"];

            title = (string)(json["title"] ?? "");
            username = (string)(json["username"] ?? "");
            first_name = (string)(json["first_name"] ?? "");
            last_name = (string)(json["last_name"] ?? "");
            all_members_are_administrators = (bool)(json["all_members_are_administrators"] ?? false);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} {2} {3} {4} {5} {6}", id, type, title, username, first_name, last_name, all_members_are_administrators);
        }
    }
}
