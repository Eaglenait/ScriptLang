using LangScriptCompilateur.Models;
using System;

namespace LangScriptCompilateur
{
    public class Compilateur
    {
        public Compilateur() { }

        public void Compile(string script)
        {
            var lexer = new Lexer(script);
            lexer.Execute();

            if (!KompilationLogger.Instance.HasFatal())
            {
                foreach (var token in lexer.Tokens)
                {
                    Console.WriteLine(string.Format("{0} - {2}", token.Word, token.Signature.ToString()));
                }
            } else {
                foreach (var log in KompilationLogger.Instance.Log)
                {
                    Console.WriteLine(string.Format("{0} - {1}", log.Item2, log.Item1));
                }
            }
        }
    }
}
