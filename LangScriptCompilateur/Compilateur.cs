﻿using LangScriptCompilateur.Models;
using System;
using System.Collections.Generic;

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

            Parser p = new Parser(lexer.Tokens);
            p.Execute();

            if(!KompilationLogger.Instance.HasFatal())
            {
                //go root
                p.Tree.Go(1);

            }
            else
            {
                foreach (var log in KompilationLogger.Instance.Log)
                {
                    Console.WriteLine(string.Format("{0} - {1}", log.Item2, log.Item1));
                }
            }
            Console.WriteLine("Compile End");
        }
    }
}
