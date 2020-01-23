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
            List<Token> tokens = new List<Token> {
                new Token{ Signature = Signature.KW_RETURN},
                new Token{ Signature = Signature.END}
            };
            SyntaxTree tree = new Parser().ParseAST(tokens);
            Assert.IsTrue(tree.TreeRoot.HasChildrens);
        }
    }
}
