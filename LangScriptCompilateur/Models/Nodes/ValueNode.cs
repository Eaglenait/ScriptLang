using LangScriptCompilateur.Models.Enums;

namespace LangScriptCompilateur.Models.Nodes
{
    public class ValueNode : SyntaxNode
    {
        public ValueNodeType ValueNodeType { get; }
        public bool IsNullable { get; }
        public bool IsNull { get; set; }
        public ValueNodeType ValueType { get; set; } 
        public object Value { get; set; }

        public ValueNode(object value, ValueNodeType type) {
            IsNullable = false;
            IsNull = false;
            Value = value;
            ValueNodeType = type;
        }
    }
}
