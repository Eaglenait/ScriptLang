using LangScriptCompilateur;
using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using NUnit.Framework;
using System.Collections.Generic;

namespace ScriptCompilateurTests.ParserTests
{
    public class SubBlocksTests
    {
        [Test]
        public void IfBlock()
        {
            //ex: if(a == 0) { return 0;}
            var ast = new List<Token>
            {
                new Token() { Signature = Signature.KW_IF },
                new Token() { Signature = Signature.LPAREN },
                new Token() { Signature = Signature.IDENTIFIER },
                new Token() { Signature = Signature.OP_EQUALS },
                new Token() { Signature = Signature.I_CONST },
                new Token() { Signature = Signature.RPAREN },
                new Token() { Signature = Signature.LBRACE },
                new Token() { Signature = Signature.KW_RETURN },
                new Token() { Signature = Signature.I_CONST },
                new Token() { Signature = Signature.END },
                new Token() { Signature = Signature.RBRACE },
            };

            List<ASTSubBlock> astSubBlocks = new List<ASTSubBlock>() {
                new ASTSubBlock
                {
                    StartIndex = 0,
                    EndIndex = 1,
                    BlockType = Signature.UNKNOWN
                },
                new ASTSubBlock
                {
                    StartIndex = 1,
                    EndIndex = 5,
                    BlockType = Signature.LPAREN
                },
                new ASTSubBlock
                {
                    StartIndex = 7,
                    EndIndex = 9,
                    BlockType = Signature.END
                },
                new ASTSubBlock
                {
                    StartIndex = 6,
                    EndIndex = 10,
                    BlockType = Signature.LBRACE
                },
            };
            var parsed = new Parser(ast).SubBlocks();

            if (parsed.Count == astSubBlocks.Count)
            {
                for (int i = 0; i < parsed.Count; i++)
                {
                    if (parsed[i].StartIndex != astSubBlocks[i].StartIndex
                        && parsed[i].BlockType != astSubBlocks[i].BlockType
                        && parsed[i].EndIndex != astSubBlocks[i].EndIndex)
                    {
                        Assert.Fail();
                    }
                }
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void BraceThenParenThenBrackets()
        {
            var ast = new List<Token>
            {
                new Token() { Signature = Signature.LBRACE },
                new Token() { Signature = Signature.RBRACE },
                new Token() { Signature = Signature.LPAREN },
                new Token() { Signature = Signature.RPAREN },
                new Token() { Signature = Signature.LBRACKET },
                new Token() { Signature = Signature.RBRACKET },
            };

            List<ASTSubBlock> astSubBlocks = new List<ASTSubBlock>() {
                new ASTSubBlock
                {
                    StartIndex = 0,
                    EndIndex = 1,
                    BlockType = Signature.LBRACE
                },
                new ASTSubBlock
                {
                    StartIndex = 2,
                    EndIndex = 3,
                    BlockType = Signature.LPAREN
                },
                new ASTSubBlock
                {
                    StartIndex = 4,
                    EndIndex = 5,
                    BlockType = Signature.LBRACKET
                }
            };

            var parsed = new Parser(ast).SubBlocks();

            if (parsed.Count == astSubBlocks.Count)
            {
                for (int i = 0; i < parsed.Count; i++)
                {
                    if (parsed[i].StartIndex != astSubBlocks[i].StartIndex
                        && parsed[i].BlockType != astSubBlocks[i].BlockType
                        && parsed[i].EndIndex != astSubBlocks[i].EndIndex)
                    {
                        Assert.Fail();
                    }
                }
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void BracketsInParenInBrace()
        {
            var ast = new List<Token>
            {
                new Token() { Signature = Signature.LBRACE },
                new Token() { Signature = Signature.LPAREN },
                new Token() { Signature = Signature.LBRACKET },
                new Token() { Signature = Signature.RBRACKET },
                new Token() { Signature = Signature.RPAREN },
                new Token() { Signature = Signature.RBRACE },
            };

            List<ASTSubBlock> astSubBlocks = new List<ASTSubBlock>() {
                new ASTSubBlock
                {
                    StartIndex = 0,
                    EndIndex = 5,
                    BlockType = Signature.LBRACE
                },
                new ASTSubBlock
                {
                    StartIndex = 1,
                    EndIndex = 4,
                    BlockType = Signature.LPAREN
                },
                new ASTSubBlock
                {
                    StartIndex = 2,
                    EndIndex = 3,
                    BlockType = Signature.LBRACKET
                }
            };

            var p = new Parser(ast);

            List<ASTSubBlock> parsed = p.SubBlocks();

            //Because for some fucked up reason Assert.AreEquals fails on parsed and astSubBlocks
            if (parsed.Count == astSubBlocks.Count)
            {
                for (int i = 0; i < parsed.Count; i++)
                {
                    if (parsed[i].StartIndex != astSubBlocks[i].StartIndex
                        && parsed[i].BlockType != astSubBlocks[i].BlockType
                        && parsed[i].EndIndex != astSubBlocks[i].EndIndex)
                    {
                        Assert.Fail();
                    }
                }
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
