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
        /// Search for a declared variable by name in the current block
        /// </summary>
        /// <returns></returns>
        private DeclarationNode SeekVariableDeclarationWithName(string name, bool isDeclaration = false)
        {
            foreach (var decl in Tree.VariableStack)
            {
                if (decl.Variable.VarName == name)
                {
                    return decl;
                }
            }

            if(isDeclaration == false)
                KompilationLogger.Instance.AddLog($"The name {name} does not exist in the current context", Severity.Fatal);
            return null;
        }

        //TODO add parsed value to a current stack use variables from stack not from returned ValueNode
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

        //TODO check if return type is correct from the function context
        private bool ParseReturnStatement(ref int at)
        {
            ReturnNode rNode = new ReturnNode();

            //first Signature is return kw second should be return value or ';'
            at++;

            //now parse the return value
            ValueNode returnValue = ParseValueNode(ref at);
            if (returnValue == null)
            {
                return false;
            }

            rNode.Value = new VarNode(returnValue);

            //now signature should be END
            at++;
            if(Ast[at].Signature != Signature.END)
            {
                return false;
            }

            Tree.Current.AddChild(rNode);
            return true;
        }

        //only supports direct comparaison. No result of operation allowed
        //TODO combinator (&& ||)
        private OperationNode ParseOperationNode(ref int at)
        {
            OperationNode cn = new OperationNode();

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

        private bool ParseIfStatement(ref int at)
        {
            var ifNode = new IfNode();
            //parse operation
            at++;

            //Parse op
            if(Ast[at].Signature != Signature.LPAREN)
            {
                return false;
            }

            at++;
            //returns at the position after the second value
            OperationNode cn = ParseOperationNode(ref at);
            if (cn == null)
            {
                return false;
            }

            ifNode.AddOperationNode(cn);

            if(Ast[at].Signature != Signature.RPAREN)
            {
                return false;
            }

            at++;
            if(Ast[at].Signature != Signature.LBRACE)
            {
                return false;
            }

            //get bounds of the {} block
            int startOfIfBlock = at;
            at++;
            var closingBraceFound = GoToClosingBrace(at);
            if(closingBraceFound.Item1 == false)
            {
                return false;
            }

            int startOfElseBlock = closingBraceFound.Item2;
            startOfElseBlock++;
            if (startOfElseBlock < Ast.Count)
            {
                if (Ast[startOfElseBlock].Signature == Signature.KW_ELSE)
                {
                    ifNode.HasElse = true;
                    ifNode.AddChild(OperationType.BLOCK);

                    startOfElseBlock++;
                    if (Ast[startOfElseBlock].Signature != Signature.LBRACE)
                    {
                        return false;
                    }

                    startOfElseBlock++;

                    if (!GoToClosingBrace(startOfElseBlock).Item1)
                    {
                        return false;
                    }
                }
            }

            Tree.Current.AddChild(ifNode);
            at = startOfIfBlock;

            return true;
        }

        //todo nullability
        //     type checking
        //     language consts (WORLD,SELF,SCRIPT)
        //     scope
        /// <summary>
        /// Parses a variable declaration
        /// </summary>
        /// <param name="at">position to start parsing in the ast</param>
        /// <returns>Declaration node</returns>
        private bool ParseVarDeclaration(ref int at)
        {
            DeclarationNode dNode = new DeclarationNode();

            //at 0 is always Signature.TYPENAME
            dNode.Variable.ValueNodeType = ValueNodeType.VARIABLE;
            string declTypeName = Ast[at].Word;

            at++;

            if (Ast[at].Signature != Signature.IDENTIFIER)
            {
                return false;
            }

            //Search tree for existing declaration
            var varDecl = SeekVariableDeclarationWithName(Ast[at].Word, true);
            if(varDecl != null)
            {
                _logger.LogFatal("Redeclaration of variable");
                return false;
            }
            dNode.Variable.VarName = Ast[at].Word;

            at++;

            if (Ast[at].Signature != Signature.OP_ASSIGN)
            {
                //not assignation case;
                _logger.LogFatal("Variable declaration without assignation");
                return false;
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
                    return false;
            }

            at++;

            if (Ast[at].Signature != Signature.END)
            {
                return false;
            }

            Tree.Current.AddChild(dNode);

            //TODO not best way of doing that
            Tree.VariableStack.Add(dNode);

            //TODO check that we return a valid position in the AST
            return true;
        }

        //TODO: differenciate top level parsing (ie var declaration) and contextual parsing (ie OperationNode parsing)
        //      we have to be able to reuse the contextual parsing while the top level parsing will always be a starting point for parsing
        public void Execute()
        {
            bool badParseFlag = false;
            Stack<ParseState> parseState = new Stack<ParseState>();
            parseState.Push(ParseState.NONE);

            //Root is 0; each time we go down a block we increment;
            int blockDepth = 0;

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
                        bool parsedVarDeclarationResult = ParseVarDeclaration(ref i);
                        if (parsedVarDeclarationResult == false)
                        {
                            badParseFlag = true;
                            continue;
                        }
                        break;

                    case Signature.KW_RETURN:
                        var parsedReturnStatementResult = ParseReturnStatement(ref i);
                        if (parsedReturnStatementResult == false)
                        {
                            badParseFlag = true;
                            continue;
                        }
                        break;

                    case Signature.KW_IF:
                        //TODO fix parse value
                        var parsedIfStatementResult = ParseIfStatement(ref i);
                        if(parsedIfStatementResult == false)
                        {
                            badParseFlag = true;
                            continue;
                        }
                        else
                        {
                            parseState.Push(ParseState.IN_IF_BLOCK);
                            blockDepth++;
                            //Go into the newly added ifNode
                            Tree.DownLast();
                            //Go into the if node first bracket group
                            Tree.Down(1);
                        }
                        break;

                    case Signature.KW_ELSE:
                        blockDepth++;
                        parseState.Push(ParseState.IN_ELSE_BLOCK);
                        blockDepth++;
                        //Go into the "else" subnode
                        Tree.Down(2);
                        break;

                    case Signature.KW_WHILE:
                        parseState.Push(ParseState.IN_WHILE_BLOCK);
                        blockDepth++;
                        //TODO
                        break;

                    case Signature.KW_FOR:
                        parseState.Push(ParseState.IN_FOR_BLOCK);
                        blockDepth++;
                        //TODO
                        break;

                    case Signature.KW_FUNCTION_DECL:
                        parseState.Push(ParseState.IN_FUNC_DECL_BLOCK);
                        blockDepth++;
                        //TODO
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

                    case Signature.RBRACE:
                        blockDepth--;
                        parseState.Pop();
                        Tree.Up();
                        break;
                }
            }
        }
        
        private (bool, int) GoToClosingBrace(int at)
        {
            bool closingBraceFound = false;
            int openBraceCount = 1;
            while(closingBraceFound == false && at <= Ast.Count)
            {
                Token current = Ast[at];
                if (current.Signature == Signature.LBRACE) openBraceCount++;
                if (current.Signature == Signature.RBRACE) openBraceCount--;
                if(openBraceCount == 0)
                {
                    closingBraceFound = true;
                    break;
                }

                if(at + 1 > Ast.Count) break;

                at++;
            }

            return (closingBraceFound, at);
        }
    }
}
