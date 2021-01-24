using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using NUnit.Framework;

namespace ScriptCompilateurTests
{
    public class SyntaxTreeTests
    {
        [Test]
        public void HasChildrens()
        {
            SyntaxTree tree = new SyntaxTree();

            tree.TreeRoot.AddChild(OperationType.NONE);

            Assert.IsTrue(tree.TreeRoot.HasChildrens);
        }

        [Test]
        public void AddChildTwice()
        {
            SyntaxTree tree = new SyntaxTree();
            tree.TreeRoot.AddChild(OperationType.NONE);
            tree.TreeRoot.AddChild(OperationType.NONE);

            Assert.IsTrue(tree.TreeRoot.Childrens.Count == 2);
        }

        [Test]
        public void AddChildToChild()
        {
            SyntaxTree tree = new SyntaxTree();
            tree.TreeRoot.AddChild(OperationType.NONE);
            var child = tree.TreeRoot.Childrens[0];
            child.AddChild(OperationType.NONE);

            Assert.IsTrue(tree.TreeRoot.Childrens[0].Childrens.Count == 1);
        }

        [Test]
        public void MultipleChildsAtFirstChild()
        {
            SyntaxTree tree = new SyntaxTree();
            tree.TreeRoot.AddChild(OperationType.NONE);
            var child = tree.TreeRoot.Childrens[0];
            child.AddChild(OperationType.NONE);
            child.AddChild(OperationType.NONE);
            child.AddChild(OperationType.NONE);

            Assert.IsTrue(tree.TreeRoot.Childrens[0].Childrens.Count == 3);
        }
    }
}
