using System;

namespace LangScriptCompilateur.Models.Enums
{
    [Flags]
    public enum ParseState : uint
    {
        NONE = 0,
        IN_IF_BLOCK = 1,
        IN_ELSE_BLOCK = 2,
        IN_FUNCTION_BLOCK = 4,
        IN_FOR_BLOCK = 8,
        IN_WHILE_BLOCK = 16,
    }
}
