using LangScriptCompilateur.Models.Enums;

namespace LangScriptCompilateur.Models.Nodes
{
    public class ReturnNode : SyntaxNode
    {
        //Return Value
        public VarNode Value { get; set; }

        public ReturnNode()
        {
            NodeType = Enums.OperationType.RETURN;
            Value = new VarNode();
        }

        #region override
        public override void AddChild(SyntaxNode child)
            => throw new System.Exception("Can't add child to this");

        public override void AddChild(OperationType nodeType)
            => AddChild(nodeType);
        #endregion
    }
}
