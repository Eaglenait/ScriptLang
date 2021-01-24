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
    }
}
