using System;
using System.Collections.Generic;
using System.Text;

namespace LangScriptCompilateur
{
    public static class ScriptToolbox
    {
        public static bool IsPrimitiveType(this string s)
        {
            foreach (string typeName in PrimitiveTypeNames())
            {
                if (typeName.Length != s.Length) continue;

                if (string.Compare(typeName, s) == 0)
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }
            return false;
        }



        public static IEnumerable<string> PrimitiveTypeNames()
        {
            yield return "string";
            yield return "int";
            yield return "float";
            yield return "list";
            yield return "bool";
        }

        public static bool IsMathOperator(this char c)
        {
            switch (c)
            {
                case '+':
                case '-':
                case '/':
                case '=':
                case '*':
                    return true;

                default:
                    return false;
            }
        }
    }
}
