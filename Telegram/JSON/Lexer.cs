using System;
using System.IO;

namespace Telegram.Parser
{
    /// <summary>
    /// Lexer that requires a string input and add alot of usefull functions for parsing.
    /// </summary>
    public class Lexer
    {
        private Token current;
        private InputStream s;

        public Lexer(string s)
        {
            current = null;
            this.s = new InputStream(s);
        }

        // Helpers
        public bool IsDigit(char c) { return c >= '0' && c <= '9'; }
        public bool IsId(char c) { return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'); }
        public bool IsOp(char c) { return "+-*/%=&|<>!".IndexOf(c) >= 0; }
        public bool IsPunc(char c) { return ",.:;[]{}()".IndexOf(c) >= 0; }
        public bool IsWhitespace(char c) { return " \t\n".IndexOf(c) >= 0; }

        /// <summary>
        /// Skips a (specific) punctuation token
        /// </summary>
        /// <param name="c">specific punctiation character you want to parse.</param>
        public void SkipPunc(char c = '\0')
        {
            if (Peek().type == TokenType.punc && (c != '\0') ? ((char)Peek().value == c) : (true)) Next();
            else throw new Exception(string.Format("Lexer> Expected punctiation, found {0}:{1}", Peek().type, Peek().value));
        }

        /// <summary>
        /// Reads while a given predicate is true.
        /// </summary>
        /// <param name="predicate">Function that takes a char and returns a bool</param>
        /// <returns>Returns the entire string for wich the predicate is satisfied.</returns>
        public string ReadWhile(Func<char, bool> predicate)
        {
            string result = "";
            while (!s.EOF() && predicate(s.Peek()))
                result += s.Next();
            return result;
        }

        /// <summary>
        /// Read a specific token.
        /// </summary>
        /// <param name="t">Token to be parsed.</param>
        public Token Read(TokenType t)
        {
            if (Peek().type == t) return Next();
            else throw new Exception(string.Format("Lexer> Expected '{0}', found {1}", t.ToString(), Peek().type.ToString()));
        }

        // Main functions
        // Parse a number that can start with a sign and can contain zero or one dot.
        /// <summary>
        /// Parse a number. This number can start with a minus-sign and contain zero or one dot.
        /// </summary>
        /// <returns>On success returns a token with TokenType.num</returns>
        public Token ReadNumber()
        {
            bool hasDot = false;
            bool start = true;
            string number = ReadWhile((char c) =>
            {
                if (c == '-')
                {
                    if (!start) return false;
                    start = false;
                    return true;
                }
                if (c == '.')
                {
                    if (hasDot) return false;
                    hasDot = true;
                    return true;
                }
                return IsDigit(c);
            });

            Token r = new Token() { type = TokenType.num };
            if (hasDot) r.value = float.Parse(number);
            else r.value = int.Parse(number);
            return r;
        }

        // Read a string encapsulated by '"'.
        /// <summary>
        /// Reads a string that is encapsulated by '"'.
        /// </summary>
        public Token ReadString()
        {
            string result = "";
            bool escaped = false;

            s.Next(); // Ignore first "

            while (!s.EOF())
            {
                char c = s.Next();
                if (escaped)
                {
                    result += c;
                    escaped = false;
                }
                else if (c == '\\') escaped = true;
                else if (c == '"') break;
                else result += c;
            }

            return new Token() { type = TokenType.str, value = result };
        }

        /// <summary>
        /// Reads the next token.
        /// </summary>
        public Token ReadNext()
        {
            ReadWhile(IsWhitespace);
            if (s.EOF())
                return null;

            // Predict what we are going to read.
            char c = s.Peek();
            if (c == '"') return ReadString();
            if (c == '-' || IsDigit(c)) return ReadNumber();
            if (IsPunc(c)) return new Token() { type = TokenType.punc, value = s.Next() };
            if (IsOp(c)) return new Token() { type = TokenType.op, value = ReadWhile(IsOp) };
            if (IsId(c)) // Identifier OR Boolean.
            {
                string tmp = ReadWhile(IsId);
                bool? result = null;
                if (tmp.ToLower() == "true") result = true;
                else if (tmp.ToLower() == "false") result = false;

                return new Token() { type = (result == null) ? TokenType.NULL : TokenType.BOOL, value = result };
            }

            s.Err("Can't handle character '" + c + "'");
            return null;
        }

        /// <summary>
        /// Returns the next token.
        /// </summary>
        /// <remarks>Resets 'current'. Reads next if current was zero. This is a little optimization trick.</remarks>
        public Token Next()
        {
            Token t = current;
            current = null;
            return t ?? ReadNext();
        }

        /// <summary>
        /// Checks what the current token is, without consuming.
        /// </summary>
        public Token Peek()
        {
            return current ?? (current = ReadNext());
        }
    }
}
