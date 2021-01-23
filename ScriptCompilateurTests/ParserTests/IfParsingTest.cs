
using LangScriptCompilateur;
using LangScriptCompilateur.Models;
using NUnit.Framework;

namespace ScriptCompilateurTests.ParserTests
{
    public class IfParsingTest
    {
        [Test]
        public void SimpleIfParse() {
            Parser p = ParserTestHelper.GetParserInstanceForScript(
                "if(a == b) {}");

            p.Execute();

            SyntaxNode node = p.Tree.PeekCurrent();
            Assert.IsTrue(node is IfNode);

            var currentNode = node as IfNode;
        }
    }
}
