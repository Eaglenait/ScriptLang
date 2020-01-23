using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Models.Nodes;
using LangScriptCompilateur.Parsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptCompilateurTests
{
    public class RulesTest
    {
        [Test]
        public void VoidReturnStatement()
        {
            List<Token> tokens = new List<Token>
            {
                new Token { Signature = Signature.KW_RETURN },
                new Token { Signature = Signature.END }
            };

            GenericRules gr = new GenericRules(tokens);
            (OperationType, SyntaxNode) operationResult = gr.Execute();

            Assert.AreEqual(operationResult.Item1, OperationType.RETURN);
            Assert.IsTrue(operationResult.Item2 is ReturnNode);
            var result = operationResult.Item2 as ReturnNode;
            Assert.AreEqual(result.Type.ToString(), TypesEnum.VOID.ToString());
        }
    }
}
