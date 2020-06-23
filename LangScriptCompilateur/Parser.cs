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

        public Parser(List<Token> ast) : this() {
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

        private ValueNode ParseValueNode(ref int at)
        {
            var value = new ValueNode();

            switch (Ast[at].Signature)
            {
                case Signature.KW_FALSE:
                case Signature.KW_TRUE:
                    value.ValueNodeType = ValueNodeType.CONST;
                    value.IsNull = false;
                    value.ValueType = TypesEnum.BOOL;

                    if (Ast[at].Signature == Signature.KW_FALSE)
                    {
                        value.Value = false;
                    }
                    else
                    {
                        value.Value = true;
                    }
                    break;

                case Signature.I_CONST:
                case Signature.F_CONST:
                    value.ValueNodeType = ValueNodeType.CONSTLITERAL;
                    value.IsNull = false;

                    if (Ast[at].Signature == Signature.I_CONST)
                    {
                        value.ValueType = TypesEnum.INT;
                        value.Value = int.Parse(Ast[at].Word);
                    }
                    else
                    {
                        value.ValueType = TypesEnum.FLOAT;
                        value.Value = float.Parse(Ast[at].Word);
                    }
                    break;

                case Signature.END:
                    value.ValueType = TypesEnum.VOID;
                    value.Value = null;
                    value.IsNull = true;
                    at--;
                    break;

                case Signature.KW_NULL:
                    value.ValueType = TypesEnum.VOID;
                    value.Value = null;
                    value.IsNull = true;
                    break;

                case Signature.STRINGLITTERAL:
                    value.Value = Ast[at].Word;
                    value.ValueType = TypesEnum.STRING;
                    value.IsNull = false;
                    break;

                case Signature.IDENTIFIER:
                    //Search tree for declaration
                    var varDecl = SeekVariableDeclarationWithName(Ast[at].Word);
                    if (varDecl == null)
                    {
                        goto default;
                    }

                    //TODO ref or copy
                    //     return const value
                    value = varDecl.Variable;
                    break;

                default:
                    at = 0;
                    return null;
            }

            return value;
        }

        private ReturnNode ParseReturnStatement(ref int at)
        {
            ReturnNode rNode = new ReturnNode
            {
                NodeType = OperationType.RETURN
            };

            //first Signature is return kw second should be return value or ';'
            at++;

            //now parse the return value
            ValueNode returnValue = ParseValueNode(ref at);
            if (returnValue == null)
            {
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

        //only supports direct comparaison. No result of operation allowed
        //TODO combinator (&& ||)
        private ComparaisonNode ParseComparaisonNode(ref int at)
        {
            ComparaisonNode cn = new ComparaisonNode();

            var l_val = ParseValueNode(ref at);

            at++;

            Signature op = Ast[at].Signature;

            at++;

            var r_val = ParseValueNode(ref at);

            at++;

            if (l_val == null || r_val == null || !op.IsComparationOperator())
            {
                at = 0;
                return null;
            }

            cn.ComparaisonType = op;
            cn.l_op = l_val;
            cn.r_op = r_val;

            return cn;
        }

        private IfNode ParseIfStatement(ref int at)
        {
            var ifNode = new IfNode();
            //parse operation
            at++;

            //Parse op
            if(Ast[at].Signature != Signature.LPAREN)
            {
                at = 0;
                return null;
            }

            at++;
            ComparaisonNode cn = ParseComparaisonNode(ref at);
            if (cn == null)
            {
                at = 0;
                return null;
            }

            at++;

            //parse else
            if(Ast[at].Signature == Signature.KW_ELSE)
            {

            }


            return ifNode;
        }

        //todo nullability
        //     type checking
        //     language consts (WORLD,SELF,SCRIPT)
        /// <summary>
        /// Parses a variable declaration
        /// </summary>
        /// <param name="at">position to start parsing in the ast</param>
        /// <returns>Declaration node</returns>
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

            //Search tree for existing declaration
            var varDecl = SeekVariableDeclarationWithName(Ast[at].Word);
            if(varDecl != null)
            {
                _logger.LogFatal("Redeclaration of variable");
                return null;
            }
            dNode.Variable.VarName = Ast[at].Word;

            at++;

            if (Ast[at].Signature != Signature.OP_ASSIGN)
            {
                //not assignation case;
                _logger.LogFatal("Variable declaration without assignation");
                at = 0;
                return null;
            }

            //Right side value
            at++;

            switch(Ast[at].Signature)
            {
                case Signature.IDENTIFIER:
                    //Search tree for existing declaration
                    var r_varDecl = SeekVariableDeclarationWithName(Ast[at].Word);
                    if(r_varDecl == null)
                    {
                        _logger.LogFatal("No declaration of right side identifier");
                        goto default;
                    }

                    string varName = dNode.Variable.VarName;
                    dNode.Variable = r_varDecl.Variable;
                    dNode.Variable.VarName = varName;
                    break;

                case Signature.I_CONST:
                case Signature.F_CONST:
                case Signature.STRINGLITTERAL:
                    dNode.Variable.ValueNodeType = ValueNodeType.CONSTLITERAL;
                    dNode.Variable.IsNull = false;
                    bool badTypeflag = false;

                    if(Ast[at].Signature == Signature.I_CONST)
                    {
                        if(int.TryParse(Ast[at].Word, out int intValue)
                           && declTypeName == "int")
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
                        if(float.TryParse(Ast[at].Word, out float floatValue)
                           && declTypeName == "float")
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
            int enclosureLevel = 0;

            for (int i = 0; i < Ast.Count; i++)
            {
                if(badParseFlag)
                {
                    _logger.LogFatal("Bad parse");
                    badParseFlag = false;
                }

                switch(Ast[i].Signature)
                {
                    //TODO drill up or down

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
                        var parsedIfStatement = ParseIfStatement(ref i);
                        enclosureLevel++;

                        if (parsedIfStatement.HasElse)
                        {

                        }
                        break;

                    case Signature.CONST_WORLD:
                        //TODO
                        break;

                    case Signature.CONST_SCRIPT:
                        //TODO
                        break;

                    case Signature.CONST_SELF:
                        //TODO
                        break;
                }
            }
        }

    }
}
