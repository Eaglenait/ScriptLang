using LangScriptCompilateur;

namespace ScriptCompilateurTests
{
    public class ParserTestHelper
    {
        public static Parser GetParserInstanceForScript(string script)
        {
            var tokens = LexerTestBase.GetTokensForScript(script);

            Parser p = new Parser(tokens);

            return p;
        }
    }
}
