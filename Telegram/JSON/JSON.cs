using Telegram.Parser;

namespace Telegram
{
    /// <summary>
    /// Main JSON Object. This object contains a single JSONObject.
    /// Can be accessed like a dictionary.
    /// </summary>
    public class JSON
    {
        // Fields
        private Lexer l;
        private JSONObject data;

        public JSON()
        {
            data = new JSONObject();
        }

        /// <summary>
        /// JSON Constructor. Uses a input string to parse the entire content.
        /// </summary>
        /// <param name="s">JSON formatted string</param>
        /// <param name="autoParse">Set to false if you don't want auto parsing.</param>
        public JSON(string s, bool autoParse = true)
        {
            if (s == null) return;
            
            data = new JSONObject();
            l = new Lexer(s);
            if (autoParse) Parse();
        }
        
        public JSONObject Data { get { return data; } }
        
        public JSONObject Parse()
        {
            // Stop if no lexer is initialised.
            if (l == null) return null;

            // Continue parsing.
            data = JSONObject.Parse(l);
            return data;
        }

        /// <summary>
        /// Get the (nested) result of the JSON object. Use numbers for 
        /// JSONArray's and words for JSONObjects. 
        /// It is just Syntactic sugar for getting elements deeper into the JSON structure.
        /// </summary>
        /// <param name="s">Access string</param>
        public object this[string s]
        {
            get
            {
                // No nesting, just return index.
                if (!s.Contains(".")) return (data != null && data.Data.ContainsKey(s))?data[s]:null;

                // Nesting. iterate over the structure.
                string[] keys = s.Split('.');
                object r = data[keys[0]];
                for (int i = 1; i < keys.Length; i++)
                {
                    int index = 0;
                    if (int.TryParse(keys[i], out index)) r = ((JSONArray)r)[index];
                    else r = ((JSONObject)r)[keys[i]];

                    if (r == null) break;
                }
                return r;
            }
        }

        public virtual bool isError() { return false; }
    }
}