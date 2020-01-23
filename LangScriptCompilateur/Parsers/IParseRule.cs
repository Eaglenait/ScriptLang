using LangScriptCompilateur.Models;
using System;
using System.Collections.Generic;

namespace LangScriptCompilateur
{
    public interface IParseRule
    {
        bool Execute(List<Token> tokens);
        List<Delegate> GetParseRules();
        void AddRule(Delegate rule);

    }
}
