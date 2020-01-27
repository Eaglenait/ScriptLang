using System;
using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Parsers;
using System.Collections.Generic;

namespace LangScriptCompilateur
{
    public class Parser
    {
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
