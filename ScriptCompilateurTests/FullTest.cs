using LangScriptCompilateur;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptCompilateurTests
{
    public class FullTest
    {
        [Test]
        public void ReturnVoidFullTest()
        {
            string script = "return;";
            Lexer lexer = new Lexer(script);


        }
    }
}
