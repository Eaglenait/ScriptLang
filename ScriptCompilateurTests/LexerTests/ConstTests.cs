using LangScriptCompilateur.Models;
using static LangScriptCompilateur.Models.Signature;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ScriptCompilateurTests.LexerTests
{
    public class ConstTests
    {
        [Test]
        public void Assignation()
        {
            List<List<Token>> tokens = new List<List<Token>>()
            {
                LexerTestBase.GetTokensForScript("int i = 0;"),
                    LexerTestBase.GetTokensForScript("float f = 1.22f;"),
                    LexerTestBase.GetTokensForScript(@"string s = ""test"";")
            };

            List<List<Signature>> signatures = new List<List<Signature>>
            {
                new List<Signature>(){TYPENAME, IDENTIFIER, OP_ASSIGN, I_CONST, END},
                    new List<Signature>(){TYPENAME, IDENTIFIER, OP_ASSIGN, F_CONST, END},
                    new List<Signature>(){TYPENAME, IDENTIFIER, OP_ASSIGN, STRINGLITTERAL, END}
            };

            for(int i = 0; i < tokens.Count; i++)
            {
                var parsed = tokens[i].Select(a => a.Signature).ToList();
                Assert.AreEqual(signatures[i], parsed);
            }
        }
    }
}
