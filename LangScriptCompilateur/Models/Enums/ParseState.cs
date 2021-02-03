using System;

namespace LangScriptCompilateur.Models.Enums
{
    public enum ParseState
    {
        NONE,
        IN_IF_BLOCK,
        IN_ELSE_BLOCK,
        IN_FUNCTION_BLOCK,
        IN_FOR_BLOCK,
        IN_WHILE_BLOCK,
        IN_FUNC_DECL_BLOCK,
    }
}
