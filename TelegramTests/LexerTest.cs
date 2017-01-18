using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telegram.Parser;

namespace TelegramTests
{
    [TestClass]
    public class LexerTest
    {
        [TestMethod]
        public void LexerNumber()
        {
            Random r = new Random();

            for (int i = 0; i < 100; i++)
                TestNumber(r.Next()); // Random positive integer

            for (int i = 0; i < 100; i++)
                TestNumber(r.Next() * -1); // Random negative integer

            for (int i = 0; i < 100; i++)
                TestNumber((float)r.NextDouble()); // Random positive float

            for (int i = 0; i < 100; i++)
                TestNumber((float)r.NextDouble() -1); // Random positive float

            {
                Lexer ls = new Lexer("-1.2.50"); // Added Point
                Token t = ls.ReadNumber();
                Assert.IsTrue(t.type == TokenType.num, "LexerNumber> Invalid Token Type");
                Assert.AreEqual(t.value, (float)-1.2, "LexerNumber> Invalid Value");
            }
            { 
                Lexer ls = new Lexer("-1-2.50"); // Added Minus
                Token t = ls.ReadNumber();
                Assert.IsTrue(t.type == TokenType.num, "LexerNumber> Invalid Token Type");
                Assert.AreEqual(t.value, -1, "LexerNumber> Invalid Value");
            }
        }

        public void TestNumber(int number)
        {
            Lexer ls = new Lexer(number.ToString());
            Token t = ls.ReadNumber();
            Assert.IsTrue(t.type == TokenType.num, "LexerNumber> Invalid Token Type");
            Assert.AreEqual((int)t.value, number, "LexerNumber> Invalid Value");
        }

        public void TestNumber(float number)
        {
            Lexer ls = new Lexer(number.ToString());
            Token t = ls.ReadNumber();
            Assert.IsTrue(t.type == TokenType.num, "LexerNumber> Invalid Token Type");
            Assert.AreEqual((float)t.value, number, "LexerNumber> Invalid Value. Found:{0}; Expected:{1}", (float)t.value, number);
        }

        [TestMethod]
        public void LexerEscapedString()
        {
            {
                Lexer ls = new Lexer("\"Hello i am Jorn\"");
                Token t = ls.ReadString();
                Assert.IsTrue(t.type == TokenType.str, "LexerEscapedString> Invalid Token Type");
                Assert.IsTrue((string)t.value == "Hello i am Jorn", "LexerEscapedString> Invalid Value");
            }

            // Escaped
            {
                Lexer ls = new Lexer("\"Hello i am \\\"Jorn\\\"\"");
                Token t = ls.ReadString();
                Assert.IsTrue(t.type == TokenType.str, "LexerEscapedString> Invalid Token Type");
                Assert.IsTrue((string)t.value == "Hello i am \"Jorn\"", "LexerEscapedString> Invalid Value");
            }
        }

        [TestMethod]
        public void LexerParse()
        {
            Lexer ls = new Lexer("\"Name\":\"Jorn\",\n\"Age\":21");

            Token t = ls.Next(); // "Name"
            Assert.IsTrue(t.type == TokenType.str && (string)t.value == "Name", "LexerParse> Invalid Token");
            t = ls.Peek();  // :    - quick check for peek
            Assert.IsTrue(t.type == TokenType.punc && (char)t.value == ':', "LexerParse> Invalid Token");
            t = ls.Next(); // :
            Assert.IsTrue(t.type == TokenType.punc && (char)t.value == ':', "LexerParse> Invalid Token");
            t = ls.Next(); // "Jorn"
            Assert.IsTrue(t.type == TokenType.str && (string)t.value == "Jorn", "LexerParse> Invalid Token");
            t = ls.Next(); // ,
            Assert.IsTrue(t.type == TokenType.punc && (char)t.value == ',', "LexerParse> Invalid Token");
            t = ls.Next(); // "Age"
            Assert.IsTrue(t.type == TokenType.str && (string)t.value == "Age", "LexerParse> Invalid Token");
            t = ls.Next(); // :
            Assert.IsTrue(t.type == TokenType.punc && (char)t.value == ':', "LexerParse> Invalid Token");
            t = ls.Next(); // 21
            Assert.IsTrue(t.type == TokenType.num && (int)t.value == 21, "LexerParse> Invalid Token");
        }
    }
}
