using LangScriptCompilateur;
using LangScriptCompilateur.Models;
using System.Collections.Generic;

namespace ScriptCompilateurTests
{
    public class LexerTestBase
    {
        public Lexer Lexer { get; set; }

        private LexerTestBase(string script)
        {
            Lexer = new Lexer(script);
        }

        private List<Token> GetTokens()
        {
            Lexer.Execute();
            return Lexer.Tokens;
        }

        public static List<Token> GetTokensForScript(string script)
        {
            return new LexerTestBase(script).GetTokens();
        }
    }
}
