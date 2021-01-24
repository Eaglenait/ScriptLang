
using LangScriptCompilateur;
using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using NUnit.Framework;
using System;

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
    }
}
