namespace LangScriptCompilateur.Models.Nodes
{
    public class ReturnNode : SyntaxNode
    {
        public TypesEnum Type { get; set; }
        public VarNode Value { get; set; }

        public ReturnNode(){
            Value = new VarNode();
        }
    }
}
