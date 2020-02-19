using System;
using System.Collections.Generic;
using System.Text;

namespace LangScriptCompilateur.Models.Nodes
{
    //represents a variable in the tree
    public class VarNode : ValueNode
    {
        public string VarName { get; set; }
        public VarNode()
        { }
    }
}
