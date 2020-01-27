using LangScriptCompilateur;
using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptCompilateurTests
{
    public class SyntaxTreeTests
    {
        [Test]
        public void HasChildrens()
        {
            SyntaxTree tree = new SyntaxTree();
            tree.AddChild(new SyntaxNode());

            Assert.IsTrue(tree.TreeRoot.HasChildrens);
        }

        [Test]
        public void AddChildTwice()
        {
            SyntaxTree tree = new SyntaxTree();
            tree.AddChild(new SyntaxNode());
            tree.AddChild(new SyntaxNode());

            Assert.IsTrue(tree.TreeRoot.Childrens.Count == 2);
        }

        [Test]
        public void AddChildToChild()
        {
            SyntaxTree tree = new SyntaxTree();
            tree.AddChild(new SyntaxNode());
            tree.Add(new SyntaxNode(), 0, 0);

            Assert.IsTrue(tree.TreeRoot.Childrens[0].Childrens.Count == 1);
        }

        [Test]
        public void AddGoAddChild()
        {
            SyntaxTree tree = new SyntaxTree();
            tree.AddChild(new SyntaxNode());
            tree.Go(0, 0);
            tree.AddChild(new SyntaxNode());

            Assert.IsTrue(tree.TreeRoot.Childrens[0].Childrens.Count == 1);
        }

        [Test]
        public void MultipleChildsAtRoot()
        {
            SyntaxTree tree = new SyntaxTree();
            tree.AddChild(new SyntaxNode());
            tree.AddChild(new SyntaxNode());

            Assert.IsTrue(tree.TreeRoot.Childrens.Count == 2);
        }

        [Test]
        public void MultipleChildsAtFirstChild()
        {
            SyntaxTree tree = new SyntaxTree();
            tree.AddChild(new SyntaxNode());
            tree.Go(0, 0);
            tree.AddChild(new SyntaxNode());
            tree.AddChild(new SyntaxNode());

            Assert.IsTrue(tree.TreeRoot.Childrens[0].Childrens.Count == 2);
        }
    }
}
