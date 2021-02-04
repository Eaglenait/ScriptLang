using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;

namespace LangScriptCompilateur
{
    //Structure
    /*
        -Parent Node
            -IfNode
                -OperationNode (content of the if)
                -SyntaxNode -> if true exec this code
                -SyntaxNode of Type Else (if HasElse = true)
                    -> if false exec this
     */
    public class IfNode : SyntaxNode
    {
        public bool HasElse { get; set; } = false;

        public void AddOperationNode(OperationNode node)
            => Childrens[0] = node;

        public IfNode()
        {
            NodeType = OperationType.IF;
            //if comparaison operation
            Childrens.Add(new OperationNode());

            //If content block
            AddChild(OperationType.BLOCK);
        }
    }
}