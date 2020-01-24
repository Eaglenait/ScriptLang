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
        public delegate SyntaxNode ReturnVoidStatement();
        private List<Token> _tokens { get; set; }

        public GenericRules(List<Token> tokens)
        {
            _tokens = tokens;

        }

        public SyntaxNode Execute()
        {
            //Reflectively executes every private method that has "Rule_" in front of its name
            var thisMethods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var method in thisMethods.Where(a => a.Name.StartsWith("Rule_")))
            {
                object methodResult = method.Invoke(this, null);

                if(!(methodResult is SyntaxNode))
                    return SyntaxNode.None();

                SyntaxNode result = (SyntaxNode)methodResult;
                switch(result.NodeType) {
                    case OperationType.RETURN:
                        return result as ReturnNode;

                    default:
                    case OperationType.NONE:
                        return SyntaxNode.None();
                }
            }

            return SyntaxNode.None();
        }

        private SyntaxNode Rule_IsReturnVoidStatement()
        {
            if(_tokens.Count == 2)
            {
                if(_tokens[0].Signature == Signature.KW_RETURN
                    &&_tokens[1].Signature == Signature.END)
                {
                    return new ReturnNode()
                    {
                        NodeType = OperationType.RETURN,
                        Type = TypesEnum.VOID,
                        Value = null
                    };
                }
            }
            return SyntaxNode.None();
        }
    }
}
