using System.Collections.Generic;
using Telegram.Parser;

namespace Telegram
{
    /// <summary>
    /// JSON Object. Has been defined as described on http://wwww.json.org
    /// </summary>
    public class JSONObject
    {
        public Dictionary<string, object> Data { get; } // Contents of the JSONObject.

        public JSONObject()
        {
            Data = new Dictionary<string, object>();
        }
        
        public static JSONObject Parse(Lexer l)
        {
            // Objects's grammar is as described on www.json.org
            // Quickref: { (string : value (, string : value)? )? }
            JSONObject result = new JSONObject();
            l.SkipPunc('{');

            if (l.Peek().IsPunc('}'))   // Empty object 
            {
                l.SkipPunc('}');
                return result;
            }

            while (l.Peek() != null)    // Not empty, continue parsing.
            {
                string name = null;
                object val = null;

                name = (string)l.Read(TokenType.str).value;                 // Get the key
                l.SkipPunc(':');

                if (l.Peek() == null) throw new System.Exception("JSONObject> Expected value");
                else if(l.Peek().IsPunc('{')) val = JSONObject.Parse(l);        // Possibly found an object 
                else if (l.Peek().IsPunc('[')) val = JSONArray.Parse(l);    // Possibly found an array
                else val = l.Next().value;                                  // Atomic value

                result.Data.Add(name, val); // Add information to the data.

                // Check for Comma, if found parse next.
                if (!l.Peek().IsPunc(',')) break;
                else l.Next();
            }

            l.SkipPunc('}'); // Eat the last curlybracket.
            return result;
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
                if (!s.Contains(".")) return (Data.ContainsKey(s)) ? Data[s] : null;

                // Nesting. iterate over the structure.
                string[] keys = s.Split('.');
                object r = Data[keys[0]];
                for (int i = 1; i < keys.Length; i++)
                {
                    int index = 0;
                    if (int.TryParse(keys[i], out index)) r = ((JSONArray)r)[index];
                    else r = ((JSONObject)r)[keys[i]];

                    if (r == null) break;
                }
                return r;
            }
            set { if (Data.ContainsKey(s)) Data[s] = value; }
        }
    }
}
