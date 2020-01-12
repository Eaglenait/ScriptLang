using LangScriptCompilateur.Models;
using System;
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
                string depth = "";
                foreach (var token in lexer.Tokens)
                {
                    if (token.Signature == Signature.LBRACE)
                        depth += ".";

                    if (token.Signature == Signature.RBRACE)
                        depth.Remove(0, 1);

                    Console.WriteLine(string.Format("{0}{1} - {2}", depth, token.Word, token.Signature.ToString()));
                }
            }
            else
            {
                foreach (var log in KompilationLogger.Instance.Log)
                {
                    Console.WriteLine(string.Format("{0} - {1}", log.Item2, log.Item1));
                }
            }
        }
    }
}
