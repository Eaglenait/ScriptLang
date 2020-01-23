using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LangScriptCompilateur
{
    public class Parser
    {
        public SyntaxTree ParseAST(List<SyntaxNode> ast)
        {
            SyntaxTree parsedTree =  new SyntaxTree();
            foreach (var node in ast)
            {
            }
            return null;
        }
    }
}
