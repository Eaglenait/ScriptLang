using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Parsers;
using System.Collections.Generic;

namespace LangScriptCompilateur
{
    public class Parser
    {
        private List<Token> Ast { get; set; }
        private List<IParseRule> Rules { get; set; }

        public Parser(List<Token> ast) {
            Ast = ast;

            Rules = new List<IParseRule>();
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

        public void Execute()
        {
            foreach (IParseRule rule in Rules)
            {
                rule.Execute();
            }
        }

    }
}
