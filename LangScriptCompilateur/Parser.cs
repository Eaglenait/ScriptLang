using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Models.Nodes;
using System.Collections.Generic;

namespace LangScriptCompilateur
{
    public class Parser
    {
        private List<Token> Ast { get; set; }
        public SyntaxTree Tree { get; private set; }

        public Parser(List<Token> ast) {
            Ast = ast;
            Tree = new SyntaxTree();
        }

        /// <summary>
        /// Extracts the position of Tokens that bounds operations 
        /// </summary>
        /// <returns></returns>
        public List<ASTSubBlock> SubBlocks()
        {
            var subBlocks = new List<ASTSubBlock>();
            int otherBlockStart = -1;
            for (int i = 0; i < Ast.Count; i++)
            {
                var block = new ASTSubBlock();

                //Starting block until first useful separator
                if (i == 0
                && !Ast[i].Signature.IsOpeningSignature())
                {
                    for (int j = i; j < Ast.Count; j++)
                    {
                        if (Ast[j].Signature == Signature.END
                        || Ast[j].Signature.IsOpeningSignature())
                        {
                            subBlocks.Add(new ASTSubBlock()
                            {
                                BlockType = Signature.UNKNOWN,
                                StartIndex = 0,
                                EndIndex = j
                            });
                            i = j;
                            break;
                        }
                    }
                }

                //if ( { [
                if (Ast[i].Signature.IsOpeningSignature())
                {
                    block.BlockType = Ast[i].Signature;
                    block.StartIndex = i;

                    //What is the corresponding closing Token
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

                    if (closingSignature == Signature.INVALID) return new List<ASTSubBlock>();

                    //Search for it
                    for (int j = i + 1; j < Ast.Count; j++)
                    {
                        if (Ast[j].Signature == closingSignature)
                        {
                            block.EndIndex = j;
                            subBlocks.Add(block);
                            otherBlockStart = -1;
                            break;
                        }
                    }
                }
                else if (Ast[i].Signature != Signature.RBRACE
                      && Ast[i].Signature != Signature.RBRACKET
                      && Ast[i].Signature != Signature.RPAREN)
                {
                    if (Ast[i].Signature == Signature.END)
                    {
                        block.BlockType = Signature.END;
                        block.EndIndex = i;
                        block.StartIndex = otherBlockStart;
                        otherBlockStart = -1;
                        subBlocks.Add(block);
                    }
                    else if (otherBlockStart == -1)
                    {
                        otherBlockStart = i;
                    }
                }
            }

            return subBlocks;
        }

        public void Execute()
        {
            var bounds = SubBlocks();

            if (bounds.Count == 0)
            {

            }

            for (int i = 0; i < Ast.Count; i++)
            {
                //Return
                if (Ast[i].Signature == Signature.KW_RETURN)
                {
                    ReturnNode rNode = new ReturnNode
                    {
                        NodeType = OperationType.RETURN
                    };

                    //Returns what
                    object returnValue = null;
                    TypesEnum returnType = TypesEnum.VOID;

                    switch (Ast[i].Signature)
                    {
                        case Signature.KW_TRUE:
                            returnValue = true;
                            returnType = TypesEnum.BOOL;
                            break;

                        case Signature.KW_FALSE:
                            returnValue = false;
                            returnType = TypesEnum.BOOL;
                            break;

                        case Signature.I_CONST:
                            returnValue = int.Parse(Ast[i].Word);
                            returnType = TypesEnum.INT;
                            break;

                        case Signature.F_CONST:
                            returnValue = float.Parse(Ast[i].Word);
                            returnType = TypesEnum.FLOAT;
                            break;

                        case Signature.END:
                            returnType = TypesEnum.VOID;
                            break;

                        case Signature.STRINGLITTERAL:
                            returnValue = Ast[i].Word;
                            returnType = TypesEnum.STRING;
                            break;
                    }

                    rNode.Value = returnValue;
                    rNode.Type = returnType;
                    Tree.AddChild(rNode);
                }
            }
        }
    }
}
