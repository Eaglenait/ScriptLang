using LangScriptCompilateur.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptCompilateurTests
{
    public static class TestConsts
    {
        public static List<Token> EmptyIfTokens()
        {
            return new List<Token>()
            {
                new Token{ Signature = Signature.KW_IF},
                new Token{ Signature = Signature.LPAREN},
                new Token{ Signature = Signature.RPAREN},
                new Token{ Signature = Signature.LBRACE},
                new Token{ Signature = Signature.RBRACE}
            };
        }

    }
}
