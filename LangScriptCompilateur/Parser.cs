using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Models.Nodes;
using System.Collections.Generic;

namespace LangScriptCompilateur
{
    public class Parser
    {
        //source
        private List<Token> Ast { get; set; }

        //destination
        public SyntaxTree Tree { get; private set; }

        public Parser(List<Token> ast) {
            Ast = ast;
            Tree = new SyntaxTree();
        }

        //todo add scope validation
        public VarNode SeekVariableDeclarationWithName(string name)
        {
            var currentCoords = Tree.currentNode;

            VarNode varNode = null;

            do
            {
                SyntaxNode parent = GoCurrentParent();
                foreach (var child in parent.GetChildrens())
                {
                    if (child is VarNode)
                    {
                        var valueChild = child as VarNode;
                        if (valueChild.VarName)
                        {
                            if(valueChild.VarName == name)
                            {
                                varNode = valueChild;
                            }
                        }
                    }
                }
            } while(Tree.currentNode.Count > 1);

            Tree.Go(currentCoords);
            return varNode;
        }

        public (int, ReturnNode) ParseReturnStatement(int at)
        {
            ReturnNode rNode = new ReturnNode
            {
                NodeType = OperationType.RETURN
            };

            //Returns what
            object returnValue = null;
            TypesEnum returnType = TypesEnum.VOID;

            //first Signature is return kw second should be return value or ';'
            at++;

            if (Ast[at].Signature != Signature.IDENTIFIER
             || Ast[at].Signature != Signature.END)
            {
                return null;
            }

            switch (Ast[at].Signature)
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
                    returnValue = int.Parse(Ast[at].Word);
                    returnType = TypesEnum.INT;
                    break;

                case Signature.F_CONST:
                    returnValue = float.Parse(Ast[at].Word);
                    returnType = TypesEnum.FLOAT;
                    break;

                case Signature.KW_NULL:
                case Signature.END:
                    returnType = TypesEnum.VOID;
                    at--;
                    break;

                case Signature.STRINGLITTERAL:
                    returnValue = Ast[at].Word;
                    returnType = TypesEnum.STRING;
                    break;

                case Signature.IDENTIFIER:
                    //Search tree for declaration
                    var varDecl = SeekVariableDeclarationWithName(Ast);
                    break;
            }

            //now signature should be END
            at++;
            if(Ast[at].Signature != Signature.END)
            {
                return null;
            }

            rNode.Value = returnValue;
            rNode.Type = returnType;

            //return to next signature
            return (at++, rNode);
        }

        public (int, DeclarationNode) ParseVarDeclaration()
        {
            
        }

        public void Execute()
        {
            for (int i = 0; i < Ast.Count; i++)
            {
                switch(Ast[i].Signature)
                {
                    case Signature.TYPENAME:

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
