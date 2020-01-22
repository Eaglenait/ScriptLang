using LangScriptCompilateur.Models.Enums;

namespace LangScriptCompilateur.Models.Nodes
{


    public class ValueNode<T> : SyntaxNode
    {
        public ValueNodeType ValueNodeType { get; }
        public bool IsNullable { get; }
        public bool IsNull { get; set; }
        T Value { get; set; }

        public ValueNode(T value, ValueNodeType type) {
            IsNullable = false;
            IsNull = false;
            Value = value;
            ValueNodeType = type;
        }
    }
}
