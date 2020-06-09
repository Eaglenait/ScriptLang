using System;
using LangScriptCompilateur;
using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Nodes;
using NUnit.Framework;

namespace ScriptCompilateurTests.ParserTests
{
    public class ReturnStatementParsingTest
    {
        [Test]
        public void SimpleReturnStatementParsing()
        {
            Parser p = ParserTestHelper.GetParserInstanceForScript("return;");
            p.Execute();

            SyntaxNode node = p.Tree.PeekCurrent();

            Assert.IsTrue(node is ReturnNode);
            var rNode = node as ReturnNode;

            Assert.AreEqual(TypesEnum.VOID, rNode.Value.ValueType);
            Assert.AreEqual(null, rNode.Value.Value);
            Assert.AreEqual(true, rNode.Value.IsNull);

            p = ParserTestHelper.GetParserInstanceForScript("return null;");
            p.Execute();

            node = p.Tree.PeekCurrent();

            Assert.IsTrue(node is ReturnNode);
            rNode = node as ReturnNode;

            Assert.AreEqual(TypesEnum.VOID, rNode.Value.ValueType);
            Assert.AreEqual(null, rNode.Value.Value);
            Assert.AreEqual(true, rNode.Value.IsNull);
        }

        [Test]
        public void ReturnConstantTest()
        {
            Parser p = ParserTestHelper.GetParserInstanceForScript("return 0;");
            p.Execute();

            SyntaxNode node = p.Tree.PeekCurrent();

            Assert.IsTrue(node is ReturnNode);
            var rNode = node as ReturnNode;
            Assert.AreEqual(TypesEnum.INT, rNode.Value.ValueType);
        }

        [Test]
        public void ReturnConstBoolTest()
        {
            Parser p = ParserTestHelper.GetParserInstanceForScript("return true;");
            p.Execute();

            SyntaxNode node = p.Tree.PeekCurrent();

            Assert.IsTrue(node is ReturnNode);
            var rNode = node as ReturnNode;
            Assert.AreEqual(TypesEnum.BOOL, rNode.Value.ValueType);
        }

        [Test]
        public void ReturnDeclaredVariableTest()
        {
            Parser p = ParserTestHelper.GetParserInstanceForScript("int i = 0; return i;");
            p.Execute();

            SyntaxNode node = p.Tree.PeekCurrent();
        }
    }
}
