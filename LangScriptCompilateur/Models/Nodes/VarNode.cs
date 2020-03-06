namespace LangScriptCompilateur.Models.Nodes
{
    //represents a variable in the tree
    public class VarNode : ValueNode
    {
        public string VarName { get; set; }
        public VarNode()
        { }

        //gets the varnode as valueNode (rvalue conversion)
        public ValueNode GetValue()
        {
            return this as ValueNode;
        }
    }
}
