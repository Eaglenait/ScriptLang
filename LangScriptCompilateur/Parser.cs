using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Models.Nodes;
using LangScriptCompilateur.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

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
                    var generic = new GenericRules(subList).Execute();
                    if (generic.Item1 != OperationType.NONE)
                    {
                        if (generic.Item1 == OperationType.RETURN)
                        {
                            parsedTree.AddChild(generic.Item2);
                            parsedTree.GoCurrentParent();
                        }
                    }
                }
            }
            return null;
        }
    }
}
