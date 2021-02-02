using LangScriptCompilateur.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace LangScriptCompilateur
{
    public class Compilateur
    {
        public Compilateur() { }

        public void Compile(string script)
        {
            Stopwatch sw = new Stopwatch();

            //Step 1 : take the string script and parse the tokens.
            //This enables to make a first syntaxic validation to see if the code is even readable or not.
            //We get a list of tokens that will be parsed into 'instructions'
            var lexer = new Lexer(script);
            sw.Restart();
            lexer.Execute();
            sw.Stop();

            Console.WriteLine();

            Console.WriteLine($"Lexer finished in: {sw.ElapsedMilliseconds} ms");
            if (!KompilationLogger.Instance.HasFatal())
            {
                foreach (var token in lexer.Tokens)
                {
                    Console.WriteLine(string.Format("{0} - {1}", token.Word, token.Signature.ToString()));
                }
            }
            else
            {
                foreach (var log in KompilationLogger.Instance.Log)
                {
                    Console.WriteLine(string.Format("{0} - {1}", log.Item2, log.Item1));
                }
            }
            Console.WriteLine();

            //Step 2: based on the tokens that we got from the lexer we parse them to extract executable instructions
            //the parsing creates a execution tree. The trees nodes are the instructions. 
            Parser p = new Parser(lexer.Tokens);
            sw.Restart();
            p.Execute();
            sw.Stop();

            Console.WriteLine($"Parser finished in: {sw.ElapsedMilliseconds} ms");

            if(!KompilationLogger.Instance.HasFatal())
            {
                Console.WriteLine("No fatal errors");
                //var nodeStrings = p.Tree.IterateAllTree().Select(e => e.NodeType.ToString());
                //foreach (var str in nodeStrings)
                //    Console.WriteLine(str);
            }
            else
            {
                foreach (var log in KompilationLogger.Instance.Log)
                    Console.WriteLine(string.Format("{0} - {1}", log.Item2, log.Item1));
            }
            Console.WriteLine("Compilation End");
        }
    }
}
