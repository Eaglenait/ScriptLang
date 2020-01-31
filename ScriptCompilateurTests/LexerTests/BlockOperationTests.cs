using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ScriptCompilateurTests.LexerTests
{
    class BlockOperationTests
    {
        [Test]
        public void ForBlock()
        {
            List<List<Token>> tokens = new List<List<Token>>() {
                LexerTestBase.GetTokensForScript("for(int i = 0; i > 0; i++)"),
            };

            List<Signature> signatures = new List<Signature> {
                Signature.KW_FOR,
                Signature.LPAREN,
                Signature.TYPENAME,
                Signature.IDENTIFIER,
                Signature.OP_ASSIGN,
                Signature.I_CONST,
                Signature.END,
                Signature.IDENTIFIER,
                Signature.OP_GREATER_THAN,
                Signature.I_CONST,
                Signature.END,
                Signature.IDENTIFIER,
                Signature.OP_INCREMENT,
                Signature.RPAREN
            };

            foreach (var token in tokens)
            {
                var parsedSignatures = token.Select(a => a.Signature).ToList();
                Assert.AreEqual(signatures, parsedSignatures);
            }

        }

        [Test]
        public void WhileBlock()
        {
            List<List<Token>> tokens = new List<List<Token>>() {
                LexerTestBase.GetTokensForScript("while (a == b) {}"),
                LexerTestBase.GetTokensForScript("while(a==b){}"),
                LexerTestBase.GetTokensForScript("while(a == b){}"),
                LexerTestBase.GetTokensForScript("while (a==b) {}"),
                LexerTestBase.GetTokensForScript("while (aasdf==basdf) {}"),
                LexerTestBase.GetTokensForScript("while( aasdf == basdf ){}"),
                LexerTestBase.GetTokensForScript("while(aasdf==basdf){}"),
            };

            List<Signature> signatures = new List<Signature> {
                Signature.KW_WHILE,
                Signature.LPAREN,
                Signature.IDENTIFIER,
                Signature.OP_EQUALS,
                Signature.IDENTIFIER,
                Signature.RPAREN,
                Signature.LBRACE,
                Signature.RBRACE
            };

            foreach (var token in tokens)
            {
                var parsedSignatures = token.Select(a => a.Signature).ToList();
                Assert.AreEqual(signatures, parsedSignatures);
            }

        }

        [Test]
        public void IfEquals()
        {
            List<List<Token>> tokens = new List<List<Token>>() {
                LexerTestBase.GetTokensForScript("if (a == b) {}"),
                LexerTestBase.GetTokensForScript("if(a==b){}"),
                LexerTestBase.GetTokensForScript("if(a == b){}"),
                LexerTestBase.GetTokensForScript("if (a==b) {}"),
                LexerTestBase.GetTokensForScript("if (aasdf==basdf) {}"),
                LexerTestBase.GetTokensForScript("if( aasdf == basdf ){}"),
                LexerTestBase.GetTokensForScript("if(aasdf==basdf){}"),
            };

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

            foreach (var token in tokens)
            {
                var parsedSignatures = token.Select(a => a.Signature).ToList();
                Assert.AreEqual(signatures, parsedSignatures);
            }
        }
    }
}
