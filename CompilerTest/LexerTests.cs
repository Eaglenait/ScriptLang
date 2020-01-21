using LangScriptCompilateur;
using LangScriptCompilateur.Models;
using NUnit.Framework;

namespace Tests
{
    public class LexerTests
    {
        Lexer l;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void LexingOfOperators()
        {
            string script = "= == + ++ && || - -- += -= .";
            l = new Lexer(script);
            l.Execute();

            var tokens = l.Tokens;

            Assert.AreEqual(tokens.Count, script.Split(' ').Length);
            Assert.AreEqual(Signature.OP_ASSIGN, tokens[0].Signature);
            Assert.AreEqual(Signature.OP_EQUALS, tokens[1].Signature);
            Assert.AreEqual(Signature.OP_PLUS, tokens[2].Signature);
            Assert.AreEqual(Signature.OP_INCREMENT, tokens[3].Signature);
            Assert.AreEqual(Signature.OP_AND, tokens[4].Signature);
            Assert.AreEqual(Signature.OP_OR, tokens[5].Signature);
            Assert.AreEqual(Signature.OP_MINUS, tokens[6].Signature);
            Assert.AreEqual(Signature.OP_DECREMENT, tokens[7].Signature);
            Assert.AreEqual(Signature.OP_PLUS_ASSIGN, tokens[8].Signature);
            Assert.AreEqual(Signature.OP_MINUS_ASSIGN, tokens[9].Signature);
            Assert.AreEqual(Signature.OP_DOT, tokens[10].Signature);
        }
    }
}