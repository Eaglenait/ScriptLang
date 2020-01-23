using System;
using System.Collections.Generic;
using System.Text;

namespace LangScriptCompilateur.Models.Nodes
{
    public class ReturnNode<T> : SyntaxNode
    {
        T Value { get; set; }
    }
}
