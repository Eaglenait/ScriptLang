using LangScriptCompilateur;
using LangScriptCompilateur.Models;
using NUnit.Framework;

namespace ScriptCompilateurTests
{
    public class FullTest
    {
        [Test]
        public void ReturnVoidFullTest()
        {
            string script = "return;";
            Lexer lexer = new Lexer(script);

            foreach(var token in lexer.Tokens)
            {
                Console.WriteLine(token.Signature.ToString());
            }

            Parser p = new Parser();
            SyntaxTree st = p.ParseAST(lexer.Tokens);
            Assert.AreEqual(st.);
        }
    }
}
