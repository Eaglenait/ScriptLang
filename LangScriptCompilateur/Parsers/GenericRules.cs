using System;
using System.Collections.Generic;
using System.Text;
using LangScriptCompilateur.Models;

namespace LangScriptCompilateur.Parsers
{
    public class GenericRules : IParseRule
    {
        public delegate bool IsReturnStatement(List<Token> tokens);

        public List<Delegate> Rules { get; private set; } = new List<Delegate>();
        private List<Token> _tokens { get; set; }

        public GenericRules()
        {
            Rules.Add(new GenericRules(IsReturnStatement));
        }

        public bool Execute()
        {
            foreach (var rule in Rules)
            {
            }
            return false;
        }

        public void AddRule(Delegate rule)
        {
            Rules.Add(rule);
        }

        public static bool IsReturnStatement(List<Token> tokens)
        {
            if (tokens.Count == 2)
            {
                if (tokens[0].Signature == Signature.KW_RETURN
                    && tokens[1].Signature == Signature.END)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
