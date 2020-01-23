using LangScriptCompilateur;
using LangScriptCompilateur.Models;
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
            Parser parser = new Parser();

            List<SyntaxNode> syntaxNodes = new List<SyntaxNode>()
            {
                new SyntaxNode()
                {
                    Childrens = new List<SyntaxNode>()
                    {
                        new SyntaxNode(),
                        new SyntaxNode()
                    }
                }
            };
            
            SyntaxTree tree = parser.ParseAST(syntaxNodes);
            Assert.IsTrue(tree.TreeRoot.HasChildrens);
        }
    }
}
