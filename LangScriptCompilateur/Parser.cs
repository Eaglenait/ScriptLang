using System;
using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Parsers;
using System.Collections.Generic;

namespace LangScriptCompilateur
{
    public class Parser
    {
        private List<Token> Ast { get; set; }

        public Parser(List<Token> ast) {
            Ast = ast;
        }

        public List<ASTSubBlock> SubBlocks()
        {
            var subBlocks = new List<ASTSubBlock>();
            int currentDepth = 0;
            var openingSign = new Stack<(Signature, int)>();
            for(int i = 0; i < Ast.Count; i++)
            {
                switch(Ast[i].Signature)
                {
                    case Signature.LBRACKET:
                    case Signature.LPAREN:
                    case Signature.LBRACE:
                        currentDepth++;
                        openingSign.Push((Ast[i].Signature, i));
                        break;
                    case Signature.RBRACKET:
                    case Signature.RPAREN:
                    case Signature.RBRACE:
                        (Signature, int) openInfo = openingSign.Pop();

                        subBlocks.Add(new ASTSubBlock()
                                {
                                    Depth = currentDepth,
                                    StartIndex = openInfo.Item2,
                                    EndIndex = i,
                                    BlockType = openInfo.Item1
                                });
                        currentDepth--;
                        break;
                }
            }

            return subBlocks;
        }

        public SyntaxTree ParseAST(List<Token> ast)
        {
            SyntaxTree parsedTree =  new SyntaxTree();

            List<Token> subList = new List<Token>();
            foreach(Token node in ast)
            {
                if (node.Signature != Signature.END)
                {
                    subList.Add(node);
                }
                else
                {
                    subList.Add(new Token { Signature = Signature.END });
                    SyntaxNode generic = new GenericRules(subList).Execute();

                    subList = new List<Token>();

                    switch(generic.NodeType) {
                        default:
                        case OperationType.NONE:
                        case OperationType.PARENT:
                        case OperationType.ASSIGNATION:
                        case OperationType.DECLARATION:
                        case OperationType.BLOCK:
                        case OperationType.VARIABLE:
                        case OperationType.CONST:
                            break;

                        case OperationType.RETURN:
                            Console.WriteLine("Add ReturnNode");
                            parsedTree.AddChild(generic);
                            return parsedTree;
                    }
                }
            }
            return null;
        }
    }
}
