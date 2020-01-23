using System;
using System.Collections.Generic;
using System.Text;

namespace LangScriptCompilateur.Models.Nodes
{
    public class ReturnNode : SyntaxNode
    {
        public TypesEnum Type { get; set; }
        public object Value { get; set; }
}
