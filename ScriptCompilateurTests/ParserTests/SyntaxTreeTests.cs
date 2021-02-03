using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptCompilateurTests.ParserTests
{
    public class SyntaxTreeTests
    {
        [Test]
        public void TestDown()
        {
            SyntaxTree st = new SyntaxTree();
            st.Current.AddChild(OperationType.BLOCK);
            st.Down(0);
            Assert.IsTrue(st.Current.Parent.NodeType == OperationType.PARENT);

            st.Current.AddChild(OperationType.BLOCK);
            st.Down(0);
            Assert.IsTrue(st.Current.Parent.NodeType == OperationType.BLOCK);
        }

        [Test]
        public void TestDown2()
        {
            SyntaxTree st = new SyntaxTree();
            st.Current.AddChild(OperationType.BLOCK);
            st.Current.AddChild(OperationType.BLOCK);
            st.Current.AddChild(OperationType.NONE);

            Assert.IsTrue(st.Current.Childrens.Count == 3);
            st.Down(2);
            Assert.IsTrue(st.Current.NodeType == OperationType.NONE);
        }

        [Test]
        public void TestUp()
        { 
            SyntaxTree st = new SyntaxTree();
            st.Current.AddChild(OperationType.NONE);
            st.Down(0);

            st.Current.AddChild(OperationType.BLOCK);
            st.Down(0);

            Assert.IsTrue(st.Current.NodeType == OperationType.BLOCK);
            st.Up();

            Assert.IsTrue(st.Current.NodeType == OperationType.NONE);
        }
    }
}
