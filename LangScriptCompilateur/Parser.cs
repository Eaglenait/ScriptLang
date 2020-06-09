using System;
using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Models.Nodes;
using System.Collections.Generic;

namespace LangScriptCompilateur
{
    public class Parser
    {
        //Source
        private List<Token> Ast { get; set; }

        private KompilationLogger _logger;

        public Parser()
        {
            _logger = KompilationLogger.Instance;
        }

        //Destination
        public SyntaxTree Tree { get; private set; } = new SyntaxTree();

        public Parser(List<Token> ast) {
            Ast = ast;
            Tree = new SyntaxTree();
        }

        //  Todo add scope validation
        //       test
        /// <summary>
        /// Search for a declared variable by name
        /// </summary>
        /// <returns></returns>
        private DeclarationNode SeekVariableDeclarationWithName(string name)
        {
            var currentCoords = Tree.CurrentNode;

            DeclarationNode declNode = null;

            do
            {
                //Changes tree coords
                SyntaxNode parent = Tree.GoCurrentParent();
                foreach (var child in parent.Childrens)
                {
                    if (child is DeclarationNode)
                    {
                        var valueChild = child as DeclarationNode;
                        if (valueChild.Variable.VarName == name)
                        {
                            declNode = valueChild;
                        }
                    }
                }
            } while(Tree.CurrentNode.Count > 1);

            //reposition current node
            Tree.Go(currentCoords.ToArray());
            return declNode;
        }

        private ReturnNode ParseReturnStatement(ref int at)
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

                    if(Ast[at].Signature == Signature.KW_FALSE)
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

                case Signature.END:
                    rNode.Value.ValueType = TypesEnum.VOID;
                    rNode.Value.Value = null;
                    rNode.Value.IsNull = true;
                    at--;
                    break;

                case Signature.KW_NULL:
                    rNode.Value.ValueType = TypesEnum.VOID;
                    rNode.Value.Value = null;
                    rNode.Value.IsNull = true;
                    break;

                case Signature.STRINGLITTERAL:
                    rNode.Value.Value = Ast[at].Word;
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

                    //TODO ref or copy
                    //     return const value
                    rNode.Value = varDecl.Variable;
                    break;

                default:
                    at = 0;
                    return null;
            }

            //now signature should be END
            at++;
            if(Ast[at].Signature != Signature.END)
            {
                at = 0;
                return null;
            }

            //return to next signature
            at++;
            return rNode;
        }

        private IfNode ParsedIfStatement(ref int at)
        {
            return null;
        }

        //todo nullability
        //     type checking
        private DeclarationNode ParseVarDeclaration(ref int at)
        {
            DeclarationNode dNode = new DeclarationNode();

            //at 0 is always Signature.TYPENAME
            dNode.Variable.ValueNodeType = ValueNodeType.VARIABLE;
            string declTypeName = Ast[at].Word;

            at++;

            if (Ast[at].Signature != Signature.IDENTIFIER)
            {
                at = 0;
                return null;
            }

            dNode.Variable.VarName = Ast[at].Word;

            at++;

            if (Ast[at].Signature != Signature.OP_ASSIGN)
            {
                //not assignation case;
                _logger.LogFatal("");
                at = 0;
                return null;
            }

            //Actual intresting part
            at++;

            switch(Ast[at].Signature)
            {
                case Signature.IDENTIFIER:
                    //Search tree for declaration
                    var varDecl = SeekVariableDeclarationWithName(Ast[at].Word);
                    //if no declaration then invalid var decl;
                    if(varDecl == null)
                    {
                        goto default;
                    }

                    //else if declaration exists then copy
                    dNode.Variable = varDecl.Variable;
                    break;

                case Signature.I_CONST:
                case Signature.F_CONST:
                case Signature.STRINGLITTERAL:
                    dNode.Variable.ValueNodeType = ValueNodeType.CONSTLITERAL;
                    dNode.Variable.IsNull = false;
                    bool badTypeflag = false;

                    if(Ast[at].Signature == Signature.I_CONST)
                    {
                        if(int.TryParse(Ast[at].Word, out int intValue))
                        {
                            dNode.Variable.Value = intValue;
                            dNode.Variable.ValueType = TypesEnum.INT;
                        }
                        else
                        {
                            badTypeflag = true;
                        }
                    }
                    else if(Ast[at].Signature == Signature.F_CONST)
                    {
                        if(float.TryParse(Ast[at].Word, out float floatValue))
                        {
                            dNode.Variable.Value = floatValue;
                            dNode.Variable.ValueType = TypesEnum.FLOAT;
                        }
                        else
                        {
                            badTypeflag = true;
                        }
                    }
                    else /*STRINGLITTERAL*/ {
                        dNode.Variable.Value = Ast[at].Word;
                        dNode.Variable.ValueType = TypesEnum.STRING;
                    }

                    if(badTypeflag)
                    {
                        _logger.LogFatal("Mismatched type");
                    }
                    break;

                case Signature.KW_TRUE:
                case Signature.KW_FALSE:
                    dNode.Variable.ValueNodeType = ValueNodeType.CONSTLITERAL;
                    dNode.Variable.IsNull = false;
                    dNode.Variable.ValueType = TypesEnum.BOOL;

                    if(Ast[at].Signature == Signature.KW_TRUE)
                    {
                        dNode.Variable.Value = true;
                    }
                    else if(Ast[at].Signature == Signature.KW_FALSE)
                    {
                        dNode.Variable.Value = false;
                    }
                    break;

                case Signature.CONST_SELF:
                case Signature.CONST_WORLD:
                case Signature.CONST_SCRIPT:
                    //Todo verify that the accessed const returns the valid type

                    dNode.Variable.ValueNodeType = ValueNodeType.VARIABLE;
                    dNode.Variable.IsNull = true;
                    break;

                case Signature.KW_NULL:
                    dNode.Variable.ValueNodeType = ValueNodeType.CONSTLITERAL;
                    dNode.Variable.IsNull = true;
                    dNode.Variable.ValueType = TypesEnum.VOID;
                    dNode.Variable.Value = null;
                    break;

                default:
                    at = 0;
                    return null;
            }

            at++;

            if (Ast[at].Signature != Signature.END)
            {
                at = 0;
                return null;
            }

            //TODO verify that it returns the correct position
            return dNode;
        }

        public void Execute()
        {
            bool badParseFlag = false;
            for (int i = 0; i < Ast.Count; i++)
            {
                if(badParseFlag)
                {
                    _logger.LogFatal("Bad parse");
                    badParseFlag = false;
                }

                switch(Ast[i].Signature)
                {
                    case Signature.TYPENAME:
                        DeclarationNode parsedVarDeclaration = ParseVarDeclaration(ref i);
                        if (i == 0)
                        {
                            badParseFlag = true;
                            continue;
                        }

                        Tree.AddChild(parsedVarDeclaration);
                        break;

                    case Signature.KW_RETURN:
                        var parsedReturnStatement = ParseReturnStatement(ref i);
                        if (i == 0
                           || parsedReturnStatement == null)
                        {
                            badParseFlag = true;
                            continue;
                        }

                        Tree.AddChild(parsedReturnStatement);
                        break;

                    case Signature.KW_IF:
                        //TODO
                        var parsedIfStatement = ParsedIfStatement(ref i);
                        break;
                }
            }
        }

    }
}
