namespace LangScriptCompilateur.Models.Nodes
{
    public class ReturnNode : SyntaxNode
    {
        //Return Value
        public VarNode Value { get; set; }

        public ReturnNode(){
            Value = new VarNode();
        }
    }
}
