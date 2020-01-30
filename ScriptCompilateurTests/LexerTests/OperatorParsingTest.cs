using LangScriptCompilateur.Models;
using NUnit.Framework;
using ScriptCompilateurTests;
using System.Collections.Generic;
using System.Linq;

namespace ScriptCompilateurTests.LexerTests
{
    public class OperatorParsingTest
    {
        [Test]
        public void Increment()
        {
            List<Token> tokens = LexerTestBase.GetTokensForScript("i++");

            List<Signature> signatures = new List<Signature>
            {
                Signature.IDENTIFIER,
                Signature.OP_INCREMENT
            };

            var parsedSignatures = tokens.Select(a => a.Signature).ToList();
            Assert.AreEqual(signatures, parsedSignatures);
        }

        [Test]
        public void Not()
        {
            List<List<Token>> tokens = new List<List<Token>>()
            {
                LexerTestBase.GetTokensForScript("!az"),
                LexerTestBase.GetTokensForScript("! az"),
            };

            List<Signature> signatures = new List<Signature>
            {
                Signature.OP_NOT,
                Signature.IDENTIFIER
            };

            foreach (var token in tokens)
            {
                var parsedSignatures = token.Select(a => a.Signature).ToList();
                Assert.AreEqual(signatures, parsedSignatures);
            }
        }

        [Test]
        public void Or()
        {
            List<List<Token>> tokens = new List<List<Token>>()
            {
                LexerTestBase.GetTokensForScript("az || bz"),
                LexerTestBase.GetTokensForScript("az|| bz"),
                LexerTestBase.GetTokensForScript("az ||bz"),
                LexerTestBase.GetTokensForScript("az||bz")
            };

            List<Signature> signatures = new List<Signature>
            {
                Signature.IDENTIFIER,
                Signature.OP_OR,
                Signature.IDENTIFIER
            };

            foreach (var token in tokens)
            {
                var parsedSignatures = token.Select(a => a.Signature).ToList();
                Assert.AreEqual(signatures, parsedSignatures);
            }
        }

        [Test]
        public void NotEquals()
        {
            List<List<Token>> tokens = new List<List<Token>>()
            {
                LexerTestBase.GetTokensForScript("az != bz"),
                LexerTestBase.GetTokensForScript("az!= bz"),
                LexerTestBase.GetTokensForScript("az !=bz"),
                LexerTestBase.GetTokensForScript("az!=bz")
            };

            List<Signature> signatures = new List<Signature>
            {
                Signature.IDENTIFIER,
                Signature.OP_NOTEQUALS,
                Signature.IDENTIFIER
            };

            foreach (var token in tokens)
            {
                var parsedSignatures = token.Select(a => a.Signature).ToList();
                Assert.AreEqual(signatures, parsedSignatures);
            }
        }
       
        [Test]
        public void Assign()
        {
            List<List<Token>> tokens = new List<List<Token>>()
            {
                LexerTestBase.GetTokensForScript("az = bz"),
                LexerTestBase.GetTokensForScript("az= bz"),
                LexerTestBase.GetTokensForScript("az =bz"),
                LexerTestBase.GetTokensForScript("az=bz")
            };

            List<Signature> signatures = new List<Signature>
            {
                Signature.IDENTIFIER,
                Signature.OP_ASSIGN,
                Signature.IDENTIFIER
            };

            foreach (var token in tokens)
            {
                var parsedSignatures = token.Select(a => a.Signature).ToList();
                Assert.AreEqual(signatures, parsedSignatures);
            }
        }

        [Test]
        public void Equals()
        {
            List<List<Token>> tokens = new List<List<Token>>()
            {
                LexerTestBase.GetTokensForScript("az == bz"),
                LexerTestBase.GetTokensForScript("az== bz"),
                LexerTestBase.GetTokensForScript("az ==bz"),
                LexerTestBase.GetTokensForScript("az==bz")
            };

            List<Signature> signatures = new List<Signature>
            {
                Signature.IDENTIFIER,
                Signature.OP_EQUALS,
                Signature.IDENTIFIER
            };

            foreach (var token in tokens)
            {
                var parsedSignatures = token.Select(a => a.Signature).ToList();
                Assert.AreEqual(signatures, parsedSignatures);
            }
        }


    }
}
