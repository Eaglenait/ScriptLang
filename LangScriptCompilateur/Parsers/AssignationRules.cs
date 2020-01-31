using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace LangScriptCompilateur.Parsers
{
    class AssignationRules : IParseRule
    {
        private List<Delegate> Rules { get; set; } = new List<Delegate>();

        public SyntaxNode Execute()
        {
            //Reflectively executes every private method that has "Rule_" in front of its name
            var thisMethods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var method in thisMethods.Where(a => a.Name.StartsWith("Rule_")))
            {
                SyntaxNode methodResult = (SyntaxNode)method.Invoke(this, null);
                if (methodResult.NodeType != OperationType.NONE)
                {
                    return methodResult;
                }
            }

            return SyntaxNode.None();
        }
    }
}
