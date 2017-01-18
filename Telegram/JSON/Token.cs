namespace Telegram.Parser
{
    public enum TokenType
    {
        punc, num, str, kw, var, op, NULL, BOOL
    }

    public class Token
    {
        public TokenType type;
        public object value;

        public Token() { }

        // Helper functions.
        public bool IsPunc(char c = '\0') { return type == TokenType.punc && ((c != '\0') ? ((char)value == c) : true); }
        public static bool IsNum(Token t) { return t.type == TokenType.num; }
        public static bool IsOp(Token t) { return t.type == TokenType.op; }
        public static bool IsKey(Token t) { return t.type == TokenType.kw; }
        public static bool IsStr(Token t) { return t.type == TokenType.str; }
        public static bool IsVar(Token t) { return t.type == TokenType.var; }
    }
}
