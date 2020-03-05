﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LangScriptCompilateur.Models.Nodes
{
    public class DeclarationNode : SyntaxNode
    {
        public VarNode Variable { get; set; }

        public DeclarationNode()
        {
            Variable = new VarNode();
        }
    }
}
