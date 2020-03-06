using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ScriptCompilateurTests.LexerTests
{
    public class StatementParsingTest
    {
        [Test]
        public void ReturnNothingStatement()
        {
            List<Token> tokens = LexerTestBase.GetTokensForScript("return;");
            List<Signature> signatures = new List<Signature>()
            {
                Signature.KW_RETURN,
                Signature.END
            };

            var parsedSignatures = tokens.Select(a => a.Signature).ToList();
            Assert.AreEqual(signatures, parsedSignatures);
        }

        [Test]
        public void ReturnIntConstStatement()
        {
            List<Token> tokens = LexerTestBase.GetTokensForScript("return 0;");
            List<Signature> signatures = new List<Signature>()
            {
                Signature.KW_RETURN,
                Signature.I_CONST,
                Signature.END
            };

            var parsedSignatures = tokens.Select(a => a.Signature).ToList();
            Assert.AreEqual(signatures, parsedSignatures);
        }

        [Test]
        public void ReturnStringConstStatement()
        {
            List<Token> tokens = LexerTestBase.GetTokensForScript(@"return ""Hello world"";");
            List<Signature> signatures = new List<Signature>()
            {
                Signature.KW_RETURN,
                Signature.STRINGLITTERAL,
                Signature.END
            };

            var parsedSignatures = tokens.Select(a => a.Signature).ToList();
            Assert.AreEqual(signatures, parsedSignatures);
        }

        [Test]
        public void ReturnIdentifierStatement()
        {
            List<Token> tokens = LexerTestBase.GetTokensForScript(@"return i;");
            List<Signature> signatures = new List<Signature>()
            {
                Signature.KW_RETURN,
                Signature.IDENTIFIER,
                Signature.END
            };

            var parsedSignatures = tokens.Select(a => a.Signature).ToList();
            Assert.AreEqual(signatures, parsedSignatures);
        }

        [Test]
        public void ReturnBoolConstStatement()
        {
            List<Token> tokens = LexerTestBase.GetTokensForScript("return true;");
            List<Signature> signatures = new List<Signature>()
            {
                Signature.KW_RETURN,
                Signature.KW_TRUE,
                Signature.END
            };

            var parsedSignatures = tokens.Select(a => a.Signature).ToList();
            Assert.AreEqual(signatures, parsedSignatures);
        }
    }
}
