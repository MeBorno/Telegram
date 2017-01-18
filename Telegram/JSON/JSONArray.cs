using System.Collections.Generic;
using Telegram.Parser;

namespace Telegram
{
    /// <summary>
    /// JSON Array. Has been defined as described on http://wwww.json.org
    /// </summary>
    public class JSONArray
    {
        public List<object> Data { get; }       // Contents of the JSONArray.

        public int Count { get { return Data.Count; } }

        public JSONArray()
        {
            Data = new List<object>();
        }

        public static JSONArray Parse(Lexer l)
        {
            // Array's grammar is as described on www.json.org
            // Quickref: [ (value (, value)?)? ]
            JSONArray result = new JSONArray();
            l.SkipPunc('[');

            if (l.Peek().IsPunc(']')) // Empty array
            {
                l.SkipPunc(']');
                return result;
            }

            while (l.Peek() != null) // Not empty, continue parsing.
            {
                object obj = null;

                if (l.Peek().IsPunc('{')) obj = JSONObject.Parse(l);        // Possibly found an object 
                else if (l.Peek().IsPunc('[')) obj = JSONArray.Parse(l);    // Possibly found an array
                else obj = l.Next().value;                                  // Atomic value

                result.Data.Add(obj); // Add information to the data.

                // Check for Comma, if found parse next.
                if (!l.Peek().IsPunc(',')) break;
                else l.Next();
            }

            l.SkipPunc(']'); // Eat the final bracket.
            return result;
        }

        /// <summary>
        /// Getter / setter just like normal array
        /// </summary>
        public object this[int i]
        {
            get { return (i < Data.Count) ? Data[i] : null; }
            set { if (i < Data.Count) Data[i] = value; }
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
                if (!s.Contains(".")) return Data[int.Parse(s)];

                // Nesting. iterate over the structure.
                string[] keys = s.Split('.');
                object r = Data[0];
                for (int i = 1; i < keys.Length; i++)
                {
                    int index = 0;
                    if (int.TryParse(keys[i], out index)) r = ((JSONArray)r)[index];
                    else r = ((JSONObject)r)[keys[i]];

                    if (r == null) break;
                }
                return r;
            }
            set { Data[int.Parse(s)] = value; }
        }
    }
}
