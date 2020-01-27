using LangScriptCompilateur;
using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Models.Nodes;
using NUnit.Framework;
using System;

namespace ScriptCompilateurTests
{
    public class FullTest
    {
        [Test]
        public void ReturnVoidFullTest()
        {
            string script = "return;";
            Lexer lexer = new Lexer(script);
            lexer.Execute();

            foreach(var token in lexer.Tokens)
            {
                Console.WriteLine(token.Signature.ToString());
            }

            Parser p = new Parser();
            SyntaxTree st = p.ParseAST(lexer.Tokens);

            ReturnNode returnNode = st.TreeRoot.Childrens[0] as ReturnNode;
            Assert.AreEqual(TypesEnum.VOID, returnNode.Type);
        }
    }
}
