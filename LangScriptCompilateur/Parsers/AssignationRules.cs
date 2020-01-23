using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LangScriptCompilateur.Parsers
{
    class AssignationRules : IParseRule
    {
        private List<Delegate> Rules { get; set; } = new List<Delegate>();

        public (OperationType, SyntaxNode) Execute()
        {
            return (OperationType.NONE, null);
        }
    }
}
