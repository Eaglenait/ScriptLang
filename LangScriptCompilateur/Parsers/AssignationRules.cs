using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using System.Linq;
using System.Reflection;

namespace LangScriptCompilateur.Parsers
{
    class AssignationRules : IParseRule
    {
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

            return SyntaxNode.None;
        }

        private SyntaxNode Rule_AssignationRule()
        {
            return SyntaxNode.None; 
        }
    }
}
