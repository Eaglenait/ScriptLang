using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;

namespace LangScriptCompilateur
{
    //Structure
    /*
        -Parent Node
            -IfNode
                -ComparaisonNode (content of the if)
                -SyntaxNode
                -SyntaxNode of Type Else (if HasElse = true)
                    ->
     */
    public class IfNode : SyntaxNode
    {
        public bool HasElse { get; set; } = false;

        public IfNode()
        {
            base.NodeType = OperationType.IF;
            //if comparaison operation
            Childrens.Add(new ComparaisonNode());

            //If content block
            Childrens.Add(new SyntaxNode(this) { 
                NodeType = OperationType.BLOCK,
            });
        }
    }
}