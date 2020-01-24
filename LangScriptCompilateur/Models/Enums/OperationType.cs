using System;
using System.Collections.Generic;
using System.Text;

namespace LangScriptCompilateur.Models.Enums
{
    public enum OperationType
    {
        NONE,
        PARENT,
        ASSIGNATION,
        DECLARATION,
        RETURN,
        BLOCK,
        IF,
        VARIABLE,
        CONST,
    }
}
