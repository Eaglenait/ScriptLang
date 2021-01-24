namespace LangScriptCompilateur.Models.Nodes
{
    //represents a variable in the tree
    public class VarNode : ValueNode
    {
        public string VarName { get; set; }
        public VarNode()
        { }

        public VarNode(ValueNode value) : base() 
        {
            ValueNodeType = value.ValueNodeType;
            ValueType = value.ValueType;
            IsNull = value.IsNull;
            Value = value.Value;
        }
    }
}
