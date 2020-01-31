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

        /// <summary>
        /// Extracts the position of Tokens that bounds operations 
        /// </summary>
        /// <returns></returns>
        public List<ASTSubBlock> SubBlocks()
        {
            var subBlocks = new List<ASTSubBlock>();
            for (int i = 0; i < Ast.Count; i++)
            {
                var block = new ASTSubBlock();

                if (Ast[i].Signature.IsOpeningSignature())
                {
                    block.BlockType = Ast[i].Signature;
                    block.StartIndex = i;

                    Signature closingSignature = Signature.INVALID;
                    switch (Ast[i].Signature)
                    {
                        case Signature.LBRACKET:
                            closingSignature = Signature.RBRACKET;
                            break;
                        case Signature.LPAREN:
                            closingSignature = Signature.RPAREN;
                            break;
                        case Signature.LBRACE:
                            closingSignature = Signature.RBRACE;
                            break;
                    }

                    if (closingSignature == Signature.INVALID) return null;

                    bool nextFound = false;
                    for (int j = i + 1; j < Ast.Count; j++)
                    {
                        if (Ast[j].Signature == closingSignature)
                        {
                            block.EndIndex = j;
                            subBlocks.Add(block);
                            break;
                        }
                        else if (Ast[j].Signature.IsOpeningSignature() && !nextFound)
                        {
                            i = j-1;
                            nextFound = true; 
                        }
                    }
                }
            }

            return subBlocks;
        }

        public void Execute()
        {
            var bounds = SubBlocks();
            foreach (IParseRule rule in Rules)
            {
                rule.Execute();
            }
        }
    }
}
