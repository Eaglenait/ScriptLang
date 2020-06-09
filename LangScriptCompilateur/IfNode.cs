using LangScriptCompilateur.Models;

namespace LangScriptCompilateur
{
    public class IfNode : SyntaxNode
    {
        public bool HasElse { get; set; } = false;
        public IfNode()
        {
            //if Operation
            Childrens.Add(new ComparaisonNode());
        }
    }
}