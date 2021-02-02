using LangScriptCompilateur;
using NUnit.Framework;

namespace ScriptCompilateurTests.ParserTests
{
    public class LongTest
    {
        [Test]
        public void FirstLongTest()
        {
            Parser p = ParserTestHelper.GetParserInstanceForScript(
                 $"int a = 0;"
                + "if(a == 0)" 
                + "{"
                + "  return 0;"
                + "}");
            p.Execute();

            Assert.IsTrue(KompilationLogger.Instance.HasFatal());
        }

    }
}
