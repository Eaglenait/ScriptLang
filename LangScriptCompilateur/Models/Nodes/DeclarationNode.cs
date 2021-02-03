using LangScriptCompilateur.Models.Enums;

namespace LangScriptCompilateur.Models.Nodes
{
    public class DeclarationNode : SyntaxNode
    {
        public VarNode Variable { get; set; }

        public DeclarationNode()
        {
            NodeType = Enums.OperationType.DECLARATION;
            Variable = new VarNode();
        }

        #region override
        public override void AddChild(SyntaxNode child)
            => throw new System.Exception("Can't add child to this");

        public override void AddChild(OperationType nodeType)
            => AddChild(nodeType);
        #endregion
    }
}
