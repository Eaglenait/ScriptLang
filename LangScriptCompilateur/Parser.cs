using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Models.Nodes;
using System.Collections.Generic;

namespace LangScriptCompilateur
{
    //todo
    //ParseVarDeclaration
    //toplevel var declaration
    public class Parser
    {
        //Source
        private List<Token> Ast { get; set; }

        //Destination
        public SyntaxTree Tree { get; private set; } = new SyntaxTree();

        public Parser(List<Token> ast) {
            Ast = ast;
            Tree = new SyntaxTree();
        }

        private void ParseTopLevelDecl()
        {
            for(int i = 0; i < Ast.Length; i++)
            {
                //case if we go down a level
                if(Ast[i].Signature.IsOpeningSignature())
                {
                    break;
                }

                if(Ast[i].Signature == Signature.TYPENAME)
                {
                    var parsedVarDeclaration = ParseVarDeclaration(i);
                    if(parsedVarDeclaration.Item1 == 0 && parsedVarDeclaration.Item2 == null)
                    {
                        //bad parse error in code
                        throw new Exception("Bad code please handle");
                    }

                    i = parsedVarDeclaration.Item1;

                    Tree.Add(parsedVarDeclaration.Item2);
                }
            }
        }

        //todo add scope validation
        //     test
        private VarNode SeekVariableDeclarationWithName(string name)
        {
            var currentCoords = Tree.CurrentNode;

            VarNode varNode = null;

            do
            {
                //Changes tree coords
                SyntaxNode parent = Tree.GoCurrentParent();
                foreach (var child in parent.Childrens)
                {
                    if (child is VarNode)
                    {
                        var valueChild = child as VarNode;
                        if (valueChild.VarName == name)
                        {
                            varNode = valueChild;
                        }
                    }
                }
            } while(Tree.CurrentNode.Count > 1);

            Tree.Go(currentCoords.ToArray());
            return varNode;
        }

        public (int, ReturnNode) ParseReturnStatement(int at)
        {
            ReturnNode rNode = new ReturnNode
            {
                NodeType = OperationType.RETURN
            };

            //first Signature is return kw second should be return value or ';'
            at++;

            switch (Ast[at].Signature)
            {
                case Signature.KW_FALSE:
                case Signature.KW_TRUE:
                    rNode.Value.ValueNodeType = ValueNodeType.CONST;
                    rNode.Value.IsNull = false;
                    rNode.Value.ValueType = TypesEnum.BOOL;

                    if(Ast[at].Signature == KW_FALSE)
                    {
                        rNode.Value.Value = false;
                    }
                    else
                    {
                        rNode.Value.Value = true;
                    }
                    break;

                case Signature.I_CONST:
                case Signature.F_CONST:
                    rNode.Value.ValueNodeType = ValueNodeType.CONSTLITERAL;
                    rNode.Value.IsNull = false;

                    if(Ast[at].Signature == Signature.I_CONST)
                    {
                        rNode.Value.ValueType = TypesEnum.INT;
                        rNode.Value.Value = int.Parse(Ast[at].Word);
                    }
                    else
                    {
                        rNode.Value.ValueType = TypesEnum.FLOAT;
                        rNode.Value.Value = float.Parse(Ast[at].Word);
                    }
                    break;

                case Signature.KW_NULL:
                case Signature.END:
                    rNode.Value.ValueType = TypesEnum.VOID;
                    rNode.Value.Value = null;
                    if(Ast[at].Signature == Signature.KW_NULL)
                    {
                        rNode.Value.IsNull = true;
                    }
                    at--;
                    break;

                case Signature.STRINGLITTERAL:
                    rNode.Value = Ast[at].Word;
                    rNode.Value.ValueType = TypesEnum.STRING;
                    rNode.Value.IsNull = false;
                    break;

                case Signature.IDENTIFIER:
                    //Search tree for declaration
                    var varDecl = SeekVariableDeclarationWithName(Ast[at].Word);
                    if(varDecl == null)
                    {
                        goto default;
                    }

                    rNode.Value = varDecl;
                    //delete var decl here
                    break;

                default:
                    return (0, null);
            }

            //now signature should be END
            at++;
            if(Ast[at].Signature != Signature.END)
            {
                return (0, null);
            }

            rNode.Value = returnValue;
            rNode.Type = returnType;

            //return to next signature
            return (at++, rNode);
        }

        public (int, DeclarationNode) ParseVarDeclaration(int at)
        {
            //at 0 is always Signature.TYPENAME
            at++;

            DeclarationNode dNode = new DeclarationNode()
            {
                NodeType = OperationType.DECLARATION;
            }

            if (Ast[at].Signature != Signature.IDENTIFIER)
            {
                return (0, null);
            }

            dNode.VarName = Ast[at].Word;

            at++;

            if (Ast[at].Signature != Signature.OP_EQUALS)
            {
                return (0, null);
            }

            at++;

            ValueNode vNode = new ValueNode();

            switch(Ast[at].Signature)
            {
                case IDENTIFIER:
                    //Search tree for declaration
                    var varDecl = SeekVariableDeclarationWithName(Ast[at].Word);
                    if(varDecl == null)
                    {
                        goto default;
                    }

                    vNode.Value = varDecl;
                    //delete var decl here
                    break;

                case I_CONST:
                case F_CONST:
                case STRINGLITTERAL:
                    vNode.ValueNodeType = ValueNodeType.CONSTLITERAL;
                    vNode.IsNull = false;

                    if(Ast[at].Signature == Signature.I_CONST)
                    {
                        if(int.TryParse(Ast[at].Word, out int intValue))
                        {
                            vNode.Value = intValue;
                            vNode.ValueType = TypesEnum.INT;
                        }
                    }
                    else if(Ast[at].Signature == Signature.F_CONST)
                    {
                        if(float.TryParse(Ast[at].Word, out float floatValue))
                        {
                            vNode.Value = floatValue;
                            vNode.ValueType = TypesEnum.FLOAT;
                        }
                    }
                    else /*STRINGLITTERAL*/ {
                        vNode.Value = Ast[at].Word;
                        vNode.ValueType = TypesEnum.STRING;
                    }

                    break;

                case KW_TRUE:
                case KW_FALSE:
                    vNode.ValueNodeType = ValueNodeType.CONSTLITERAL;
                    vNode.IsNull = false;
                    vNode.ValueType = TypesEnum.BOOL;

                    if(Ast[at].Signature == Signature.KW_TRUE)
                    {
                        vNode.Value = true;
                    }
                    else if(Ast[at].Signature == Signature.KW_FALSE)
                    {
                        vNode.Value = false;
                    }
                    break;

                case CONST_SELF:
                case CONST_WORLD:
                case CONST_SCRIPT:
                    //Todo verify that the accessed const returns the valid type

                    vNode.ValueNodeType = ValueNodeType.VARIABLE;
                    vNode.IsNull = true;
                    break;

                case KW_NULL:
                    vNode.ValueNodeType = ValueNodeType.CONSTLITERAL;
                    vNode.IsNull = true;
                    vNode.ValueType = TypesEnum.VOID;
                    vNode.Value = null;
                    break;

                default:
                    return (0, null);
            }

            at++;

            if (Ast[at].Signature != Signature.END)
            {
                return (0, null);
            }

            return dNode;
        }

        public void Execute()
        {
            ParseTopLevelDecl();

            for (int i = 0; i < Ast.Count; i++)
            {
                switch(Ast[i].Signature)
                {
                    case Signature.TYPENAME:
                        ParseVarDeclaration(i);
                        break;

                    case Signature.KW_RETURN:
                        var parsedReturnStatment = ParseReturnStatement();
                        i = parsedReturnStatement.Item1;

                        if(parsedReturnStatment.Item2 != null)
                        {
                            Tree.AddChild(parsedReturnStatment.Item2);
                            Tree.GoCurrentParent();
                        }
                        break;
                }

                //Return
                if (Ast[i].Signature == Signature.KW_RETURN)
                {
                    Tree.AddChild(rNode);
                }
            }
        }
    }
}
