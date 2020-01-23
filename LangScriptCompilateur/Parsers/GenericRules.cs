using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Models.Nodes;

namespace LangScriptCompilateur.Parsers
{
    public class GenericRules : IParseRule
    {
        public delegate (OperationType, SyntaxNode) ReturnVoidStatement();
        private List<Token> _tokens { get; set; }

        public GenericRules(List<Token> tokens)
        {
            _tokens = tokens;

        }

        public (OperationType, SyntaxNode) Execute()
        {
            var thisMethods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var method in thisMethods.Where(a => a.Name.StartsWith("Rule_")))
            {
                Object methodResult = method.Invoke(this, null);
                (OperationType, SyntaxNode) statement = ((OperationType, SyntaxNode))methodResult;
                if (statement.Item1 != OperationType.NONE)
                {
                    return statement;
                }
            }

            return (OperationType.NONE, null);
        }

        private (OperationType, SyntaxNode) Rule_IsReturnVoidStatement()
        {
            if(_tokens.Count == 2)
            {
                if(_tokens[0].Signature == Signature.KW_RETURN
                    &&_tokens[1].Signature == Signature.END)
                {
                    return (OperationType.RETURN, new ReturnNode()
                    {
                        Type = TypesEnum.VOID,
                        Value = null
                    });
                }
            }
            return (OperationType.NONE, null);
        }
    }
}
