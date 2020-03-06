using LangScriptCompilateur.Models.Enums;

namespace LangScriptCompilateur.Models.Nodes
{
    //represents a value in code
    public class ValueNode
    {
        //const or variable
        public ValueNodeType ValueNodeType { get; set; }
        //Type of value
        public TypesEnum ValueType { get; set; }
        public bool IsNull { get; set; }
        public object Value { get; set; }
    }
}
