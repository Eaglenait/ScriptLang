using System.Collections.Generic;
using LangScriptCompilateur;
using LangScriptCompilateur.Models;
using NUnit.Framework;

namespace ScriptCompilateurTests.ParserTests
{
    public class SubBlocksTests
    {
        [Test]
        public void SubBlocks()
        {
            List<Token> ast = new List<Token>
            {
                new Token{ Signature.LBRACE },
                new Token{ Signature.LPAREN },
                new Token{ Signature.LBRACKET },
                new Token{ Signature.RBRACKET },
                new Token{ Signature.RPAREN },
                new Token{ Signature.RBRACE },
            };

            List<ASTSubBlock> parsed = new List<ASTSubBlock>()
            {
                new ASTSubBlock
                {
                    Depth = 0,
                    StartIndex = 0,
                    EndIndex = 5,
                    BlockType = Signature.LBRACE
                },
                new ASTSubBlock
                {
                    Depth = 1,
                    StartIndex = 1,
                    EndIndex = 4,
                    BlockType = Signature.LPAREN
                },
                new ASTSubBlock
                {
                    Depth = 2,
                    StartIndex = 2,
                    EndIndex = 3,
                    BlockType = Signature.LBRACKET
                }
            };

            var p = new Parser(ast);

            List<ASTSubBlock> astBusBlocks = p.SubBlocks();

            Assert.AreEqual(parsed, ast);
        }
    }
}
