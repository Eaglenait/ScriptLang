using System.Collections.Generic;

namespace LangScriptCompilateur.Models
{
    public class SyntaxNode<T>
    {
        T Type { get; set; }
    }

    public class SyntaxNode
    {
        public SyntaxNode Parent { get; set; }
        public List<SyntaxNode> Childrens { get; set; } = new List<SyntaxNode>();
        public bool HasChildrens
        {
            get
            {
                if (Childrens == null)
                {
                    return false;
                }

                if (Childrens.Count != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
