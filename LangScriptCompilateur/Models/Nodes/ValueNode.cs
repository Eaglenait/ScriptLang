using LangScriptCompilateur.Models.Enums;

namespace LangScriptCompilateur.Models.Nodes
{
    //represents a value in code
    public class ValueNode
    {
        public ValueNodeType ValueNodeType { get; }
        public TypesEnum ValueType {get;set;}
        public bool IsNull { get; set; }
        public object Value { get; set; }
    }
}
