using LangScriptCompilateur;
using NUnit.Framework;

namespace ScriptCompilateurTests.ParserTests
{
    public class VarDeclarationTests
    {

        [Test]
        public void SeekVarDeclarationTest()
        {
            string script = 
                @"int i = 0;
                  int i = 2;";
            Parser p = ParserTestHelper.GetParserInstanceForScript(script);
            p.Execute();

        }
    }
}
