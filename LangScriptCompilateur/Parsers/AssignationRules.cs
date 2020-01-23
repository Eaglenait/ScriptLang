using LangScriptCompilateur.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LangScriptCompilateur.Parsers
{
    class AssignationRules : IParseRule
    {
        private List<Delegate> Rules { get; set; } = new List<Delegate>();

        public bool Execute(List<Token> tokens)
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

        public List<Delegate> GetParseRules()
        {
            throw new NotImplementedException();
        }
    }
}
