using LangScriptCompilateur;
using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Models.Nodes;
using NUnit.Framework;

namespace ScriptCompilateurTests.ParserTests
{
    public class LongTest
    {
        [Test]
        public void LongTest1()
        {
            Parser p = ParserTestHelper.GetParserInstanceForScript(
                 $"int a = 0;"
                + "if(a == 0)" 
                + "{"
                + "  return 0;"
                + "}");
            p.Execute();
            p.Tree.GoRoot();

            var root = p.Tree;

            //Go to first declaration node
            root.Down(0);
            Assert.IsTrue(root.Current.NodeType == OperationType.DECLARATION);

            //Back to root
            root.Up();
            //down to ifNode
            root.Down(1);
            Assert.IsTrue(root.Current.NodeType == OperationType.IF);
            Assert.IsTrue(root.Current.Childrens.Count == 2);

            //down to if block node
            root.DownLast();
            //down to return node
            root.DownLast();
            Assert.IsTrue(root.Current.NodeType == OperationType.RETURN);

            Assert.IsFalse(KompilationLogger.Instance.HasFatal());
        }

        [Test]
        public void LongTest2()
        {
            Parser p = ParserTestHelper.GetParserInstanceForScript(
                "int a = 0;" +
                "if(a == 0)" +
                "{" +
                "   int b = 2;" +
                "   return 1;" +
                "}" +
                "else" +
                "{" +
                "   int b = 3;" +
                "   return 0;" +
                "}");
            p.Execute();
            p.Tree.GoRoot();

            var root = p.Tree;

            //Go to first declaration node
            root.Down(0);
            Assert.IsTrue(root.Current.NodeType == OperationType.DECLARATION);
            var aDeclNode = root.Current as DeclarationNode;
            Assert.AreEqual((int)aDeclNode.Variable.Value, 0);

            //Back to root
            root.Up();
            //Down to ifNode
            root.Down(1);
            Assert.IsTrue(root.Current.NodeType == OperationType.IF);
            Assert.IsTrue(root.Current.Childrens.Count == 3);

            //Down to if block
            root.Down(1);
            Assert.IsTrue(root.Current.NodeType == OperationType.BLOCK);
            Assert.IsTrue(root.Current.Childrens.Count == 2);
            Assert.IsTrue(root.Current.Childrens[0].NodeType == OperationType.DECLARATION);

            //Go back up to the ifnode and down to the else block
            root.Up();
            root.Down(1);
            Assert.IsTrue(root.Current.NodeType == OperationType.BLOCK);
            Assert.IsTrue(root.Current.Childrens.Count == 2);
            Assert.IsTrue(root.Current.Childrens[0].NodeType == OperationType.DECLARATION);
        }
    }
}
