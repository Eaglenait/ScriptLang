using LangScriptCompilateur.Models;
using NUnit.Framework;
using ScriptCompilateurTests;
using System.Collections.Generic;
using System.Linq;


namespace Tests
{
    public class OperatorParsingTest
    {
        [Test]
        public void Equals()
        {
            List<Token> tokens = LexerTestBase.GetTokensForScript("==");
            List<Signature> signatures = new List<Signature>
            {
                Signature.OP_EQUALS
            };


            List<Signature> parsedSignatures = tokens.Select(t => t.Signature).ToList();

            Assert.AreEqual(signatures, parsedSignatures);
        }

        [Test]
        public void IfEquals()
        {
            List<Token> tokens = LexerTestBase.GetTokensForScript("if (a == b) {}");

            List<Signature> signatures = new List<Signature> {
                Signature.KW_IF,
                Signature.LPAREN,
                Signature.IDENTIFIER,
                Signature.OP_EQUALS,
                Signature.IDENTIFIER,
                Signature.RPAREN,
                Signature.LBRACE,
                Signature.RBRACE
            };
            List<Signature> parsedSignatures = tokens.Select(t => t.Signature).ToList();

            Assert.AreEqual(signatures, parsedSignatures);
        }
    }
}