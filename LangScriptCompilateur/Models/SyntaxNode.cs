using System.Collections.Generic;

namespace LangScriptCompilateur.Models
{
    public class SyntaxNode
    {
        public SyntaxNode Parent { get; set; }
        public List<SyntaxNode> Childrens { get; set; }
    }
}
