using LangScriptCompilateur.Models;
using System.Collections.Generic;
using static LangScriptCompilateur.Models.Enums.Signature;

namespace ScriptCompilateurTests
{
    public static class TestConsts
    {
        public static List<Token> EmptyIfTokens()
        {
            return new List<Token>()
            {
                new Token{ Signature = KW_IF },
                new Token{ Signature = LPAREN },
                new Token{ Signature = RPAREN },
                new Token{ Signature = LBRACE },
                new Token{ Signature = RBRACE }
            };
        }
    }
}
