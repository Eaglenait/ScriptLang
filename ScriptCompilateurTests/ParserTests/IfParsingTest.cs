using LangScriptCompilateur;
using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using NUnit.Framework;
using System.Linq;

namespace ScriptCompilateurTests.ParserTests
{
    public class IfParsingTest
    {
        [Test]
        public void IfParse() {
            Parser p = ParserTestHelper.GetParserInstanceForScript(
                "if(0 == 1) {}");

            p.Execute();
            var ifNode = p.Tree.TreeRoot.Childrens[0] as IfNode;

            Assert.IsNotNull(ifNode);
            Assert.IsFalse(ifNode.HasElse);
            Assert.AreEqual(ifNode.Childrens.Count, 2);

            Assert.IsTrue(ifNode.Childrens[0] is ComparaisonNode);
            Assert.IsTrue(ifNode.Childrens[1].NodeType == OperationType.BLOCK);
        }

        [Test]
        public void IfElseParse()
        {
            Parser p = ParserTestHelper.GetParserInstanceForScript(
                "if(0 == 1) {} " +
                "else {}");

            p.Execute();
            var ifNode = p.Tree.TreeRoot.Childrens[0] as IfNode;

            Assert.IsNotNull(ifNode);
            Assert.IsTrue(ifNode.HasElse);
            Assert.AreEqual(ifNode.Childrens.Count, 3);

            Assert.IsTrue(ifNode.Childrens[0] is ComparaisonNode);
            Assert.IsTrue(ifNode.Childrens[1].NodeType == OperationType.BLOCK);
            Assert.IsTrue(ifNode.Childrens[2].NodeType == OperationType.BLOCK);
        }

        [Test]
        public void IfWithUndeclaredVariable()
        {
            Parser p = ParserTestHelper.GetParserInstanceForScript(
                        "if(a == 1) {}");
            p.Execute();

            Assert.IsTrue(KompilationLogger.Instance.HasFatal());
        }

        [Test]
        public void IfElseWithContent()
        {
            Parser p = ParserTestHelper.GetParserInstanceForScript(
            "if (1 == 1) {" +
            "   return 0;" +
            "}" +
            "else" +
            "{" +
            "   return 1;" +
            "}");

            p.Execute();

            IfNode ifnode = p.Tree.TreeRoot.Childrens[0] as IfNode;

            //main block
            Assert.IsTrue(ifnode.Childrens[1].HasChildrens);
            //else block
            Assert.IsTrue(ifnode.Childrens[2].HasChildrens);
        }
    }
}
