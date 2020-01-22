namespace LangScriptCompilateur.Models.Nodes
{
    public class IfNode<T,J>
    {
        ValueNode<T> LeftOperator { get; set; }
        ValueNode<J> RightOperator { get; set; }


    }
}
