using System;
using System.Collections.Generic;
using System.Text;

namespace LangScriptCompilateur.Models.Nodes
{
    public class DeclarationNode : SyntaxNode
    {
        public string VarName { get; set; } 
        public DeclarationNode()
        {
            
        }
    }
}
