using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using System;
using System.Collections.Generic;

namespace LangScriptCompilateur
{
    public interface IParseRule
    {
        (OperationType, SyntaxNode) Execute();
    }
}
