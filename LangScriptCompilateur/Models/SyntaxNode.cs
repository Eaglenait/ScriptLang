using LangScriptCompilateur.Models.Enums;
using System.Collections.Generic;

namespace LangScriptCompilateur.Models
{
    public class SyntaxNode
    {
        public OperationType NodeType { get; set; }
        public SyntaxNode Parent { get; set; }
        public List<SyntaxNode> Childrens { get; set; } = new List<SyntaxNode>();

        public static SyntaxNode None { get { return new SyntaxNode() { NodeType = OperationType.NONE}; } }

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
