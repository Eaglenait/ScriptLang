using LangScriptCompilateur.Models.Enums;
using System.Collections.Generic;

namespace LangScriptCompilateur
{
    public static class ScriptToolbox
    {
        public static bool IsComparationOperator(this Signature s)
        {
            switch (s)
            {
                case Signature.OP_EQUALS:
                case Signature.OP_NOTEQUALS:
                case Signature.OP_GREATER_THAN_OR_EQUALS:
                case Signature.OP_LESS_THAN_OR_EQUALS:
                case Signature.OP_GREATER_THAN:
                case Signature.OP_LESS_THAN:
                    return true;
                default:
                    return false;
            }
        }

        //Returns true if the signature can be used in a mathematical operation
        //Returns false otherwise
        public static bool IsValidMathOperationSignature(this Signature s)
        {
            switch(s)
            {
                case Signature.OP_PLUS_ASSIGN:
                case Signature.OP_MINUS_ASSIGN:
                case Signature.OP_MUL_ASSIGN:
                case Signature.OP_INCREMENT:
                case Signature.OP_DECREMENT:
                case Signature.OP_EQUALS:
                case Signature.OP_NOTEQUALS:
                case Signature.OP_ASSIGN:
                case Signature.LPAREN:
                case Signature.RPAREN:
                case Signature.OP_GREATER_THAN_OR_EQUALS:
                case Signature.OP_LESS_THAN_OR_EQUALS:
                case Signature.OP_GREATER_THAN:
                case Signature.OP_LESS_THAN:
                case Signature.OP_NOT:

                case Signature.I_CONST:
                case Signature.F_CONST:
                case Signature.OP_PLUS:
                case Signature.OP_MINUS:
                case Signature.OP_MUL:
                case Signature.KW_TRUE:
                case Signature.KW_FALSE:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsStructuralKeyWord(this Signature s)
        {
            switch (s)
            {
                case Signature.KW_IF:
                case Signature.KW_SWITCH:
                case Signature.KW_WHILE:
                case Signature.KW_FOR:
                case Signature.KW_DO:
                case Signature.KW_FUNCTION_DECL:
                case Signature.KW_ENUM_DECL:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// returns true if signature if an opening parenthesis bracket or curly bracket
        /// </summary>
        public static bool IsOpeningSignature(this Signature s)
        {
            switch (s)
            {
                case Signature.LBRACKET:
                case Signature.LPAREN:
                case Signature.LBRACE:
                    return true;
                default:
                    return false;
            }
        }

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
