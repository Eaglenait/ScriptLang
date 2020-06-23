using LangScriptCompilateur;
using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Nodes;
using NUnit.Framework;

namespace ScriptCompilateurTests.ParserTests
{
    public class VarDeclarationTests
    {

        [Test]
        public void VarRedeclarationTest()
        {
            //Script should fail because of redeclaration
            string script =
                @"int i = 0;
                  int i = 2;";
            Parser p = ParserTestHelper.GetParserInstanceForScript(script);
            p.Execute();

            var logger = KompilationLogger.Instance;
            Assert.IsTrue(logger.HasFatal());
        }

        [Test]
        public void ConstVarCopyDeclarationTest()
        {
            string script =
                @"int i = 0;
                  int j = i;";
            Parser p = ParserTestHelper.GetParserInstanceForScript(script);
            p.Execute();

            //peek last
            DeclarationNode jDeclaration = p.Tree.PeekCurrent() as DeclarationNode;

            int jValue = (int)jDeclaration.Variable.Value;
            //verify that i = j
            Assert.AreEqual(jValue, 0);
        }

        [Test]
        public void InvalidConstVarCopyDeclarationTest()
        {
            string script =
                @"int i = 0;
                  float j = i;";
            Parser p = ParserTestHelper.GetParserInstanceForScript(script);
            p.Execute();

            //peek last
            DeclarationNode jDeclaration = p.Tree.PeekCurrent() as DeclarationNode;

            //int jValue = (int)jDeclaration.Variable.Value;
        }
    }
}
