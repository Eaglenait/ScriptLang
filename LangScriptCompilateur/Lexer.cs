using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangScriptCompilateur
{
    public class Lexer
    {
        private string Script { get; set; }
        private KompilationLogger Logger;

        public List<Token> Tokens { get; private set; }

        private Lexer() { }

        public Lexer(string script)
        {
            Script = script;
            Logger = KompilationLogger.Instance;
            Logger.Log.Clear();
        }

        public bool Execute()
        {
            Tokens = new List<Token>();

            ExtractTokens();

            if (Tokens != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ExtractTokens()
        {
            if (string.IsNullOrEmpty(Script))
            {
                Logger.LogFatal("Invalid Script file");
                return;
            }

            if (!AreOpenCloseStatementsPaired()) return;


            for (int i = 0; i < Script.Length; i++)
            {
                switch (Script[i])
                {
                    case ' ':
                    case '\r':
                    case '\n':
                    case '\t':
                        break;
                    case '(':
                        Tokens.Add(new Token() { Signature = Signature.LPAREN });
                        break;
                    case ')':
                        Tokens.Add(new Token() { Signature = Signature.RPAREN });
                        break;
                    case '{':
                        Tokens.Add(new Token() { Signature = Signature.LBRACE });
                        break;
                    case '}':
                        Tokens.Add(new Token() { Signature = Signature.RBRACE });
                        break;
                    case '[':
                        Tokens.Add(new Token() { Signature = Signature.LBRACKET });
                        break;
                    case ']':
                        Tokens.Add(new Token() { Signature = Signature.RBRACKET });
                        break;
                    case ',':
                        Tokens.Add(new Token() { Signature = Signature.COMMA });
                        break;
                    case '#':
                        Tokens.Add(new Token() { Signature = Signature.PREPROCESSOR });
                        break;
                    case ';':
                        Tokens.Add(new Token() { Signature = Signature.END });
                        break;
                    case '.':
                        Tokens.Add(new Token() { Signature = Signature.OP_DOT });
                        break;
                    case '>':
                            if(Script[i + 1] == '=')
                            {
                                Tokens.Add(new Token() {
                                    Signature = Signature.OP_GREATER_THAN_OR_EQUALS
                                });

                                i += 2;
                            }
                            else
                            {
                                Tokens.Add(new Token() {
                                    Signature = Signature.OP_GREATER_THAN
                                });
                            }
                        break;

                    case '<':
                        if (IsNextIndexValid(i))
                        {
                            if(Script[i+1] == '=')
                            {
                                Tokens.Add(new Token() {
                                    Signature = Signature.OP_LESS_THAN_OR_EQUALS
                                });

                                i += 2;
                            }
                            else
                            {
                                Tokens.Add(new Token() {
                                    Signature = Signature.OP_LESS_THAN
                                });
                            }
                        }
                        break;
                    case '+':
                        if (IsNextIndexValid(i))
                        {
                            switch (Script[i + 1])
                            {
                                case '+':
                                    Tokens.Add(new Token() { Signature = Signature.OP_INCREMENT });
                                    i += 1;
                                    break;
                                case '=':
                                    Tokens.Add(new Token() { Signature = Signature.OP_PLUS_ASSIGN });
                                    i += 1;
                                    break;
                                default:
                                    Tokens.Add(new Token() { Signature = Signature.OP_PLUS });
                                    break;
                            }
                        }
                        break;
                    case '-':
                        if (IsNextIndexValid(i))
                        {
                            switch (Script[i + 1])
                            {
                                case '-':
                                    Tokens.Add(new Token() { Signature = Signature.OP_DECREMENT });
                                    i += 1;
                                    break;
                                case '=':
                                    Tokens.Add(new Token() { Signature = Signature.OP_MINUS_ASSIGN });
                                    i += 1;
                                    break;
                                default:
                                    Tokens.Add(new Token() { Signature = Signature.OP_MINUS });
                                    break;
                            }
                        }
                        break;
                    case '=':
                        if (IsNextIndexValid(i))
                        {
                            if (Script[i + 1] == '=')
                            {
                                Tokens.Add(new Token() { Signature = Signature.OP_EQUALS });
                                i += 1;
                                if (IsNextIndexValid(i))
                                    if (char.IsWhiteSpace(Script[i + 1]))
                                    {
                                        i += 1;
                                    }
                                break;
                            }
                            else
                            {
                                Tokens.Add(new Token() { Signature = Signature.OP_ASSIGN });
                            }
                        }

                        break;
                    case '!':
                        if (IsNextIndexValid(i))
                        {
                            if (Script[i + 1] == '=')
                            {
                                Tokens.Add(new Token() { Signature = Signature.OP_NOTEQUALS });
                                i += 1;
                                break;
                            }
                            else
                            {
                                Tokens.Add(new Token() { Signature = Signature.OP_NOT });
                            }
                        }

                        break;
                    case '&':
                        if (IsNextIndexValid(i))
                        {
                            if (Script[i + 1] == '&')
                            {
                                Tokens.Add(new Token() { Signature = Signature.OP_AND });
                                Console.WriteLine("returns at: "+ (i+2) + " char: " + Script[i+2]);
                                i += 1;
                                break;
                            }
                            else
                            {
                                Logger.LogFatal("Invalid symbol");
                                return;
                            }
                        }
                        break;
                    case '|':
                        if (IsNextIndexValid(i))
                        {
                            if (Script[i + 1] == '|')
                            {
                                Tokens.Add(new Token() { Signature = Signature.OP_OR });
                                i += 1;
                                break;
                            }
                            else
                            {
                                Logger.LogFatal("Invalid symbol");
                                return;
                            }
                        }
                        break;
                    case '"':
                        var word = new StringBuilder();
                        for (int j = i + 1; j < Script.Length; j++)
                        {
                            if (Script[j] == '"')
                            {
                                i = j;
                                break;
                            }
                            else
                            {
                                word.Append(Script[j]);
                            }
                        }

                        Tokens.Add(new Token()
                        {
                            Word = word.ToString(),
                            Signature = Signature.STRINGLITTERAL
                        });
                        break;
                    default:
                        if (char.IsDigit(Script[i]))
                        {
                            int numberScanResult = ScanNumber(i);

                            if (numberScanResult >= Script.Length
                             || numberScanResult == -1)
                            {
                                return;
                            }
                            else
                            {
                                i = numberScanResult;
                            }
                        }
                        else if (char.IsLetter(Script[i]))
                        {
                            int identifierScanResult = ScanIdentifier(i);
                            if (identifierScanResult >= Script.Length)
                            {
                                return;
                            }
                            else
                            {
                                i = identifierScanResult;
                            }
                        }
                        else
                        {
                            Logger.LogFatal("Illegal char: " + Script[i]);
                            return;
                        }
                        break;
                }
            }
        }

        //handles const numbers
        private int ScanNumber(int i)
        {
            var number = new StringBuilder();
            bool isFloat = false;
            for (int j = i; j < Script.Length; j++)
            {
                switch (Script[j])
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        number.Append(Script[j]);
                        break;

                    case '.':
                        if (!number.ToString().Contains('.'))
                        {
                            number.Append(Script[j]);
                        }
                        else
                        {
                            Logger.LogFatal("invalid const float: multiple separators");
                            return -1;
                        }
                        break;

                    case 'f':
                        if (!number.ToString().Contains('.'))
                        {
                            Logger.LogFatal("invalid const float: no separator");
                            return -1;
                        }
                        else if (number.ToString().Contains('f'))
                        {
                            Logger.LogFatal("invalid const float: multiple f");
                            return -1;
                        }
                        else
                        {
                            number.Append('f');
                            isFloat = true;
                        }
                        break;

                    default:
                        if (!Script[j].IsMathOperator()
                           || Script[j] == ';'
                           || char.IsWhiteSpace(Script[j]))
                        {
                            Signature s = Signature.I_CONST;
                            if (isFloat)
                            {
                                s = Signature.F_CONST;
                            }

                            Tokens.Add(new Token()
                            {
                                Word = number.ToString(),
                                Signature = s
                            });

                            return j - 1;
                        }
                        else
                        {
                            return -1;
                        }
                }
            }

            return -1;
        }

        private int ScanIdentifier(int i)
        {
            var identifier = new StringBuilder();
            int j = 0;
            for (j = i; j < Script.Length; j++)
            {
                if (char.IsLetterOrDigit(Script[j]))
                {
                    identifier.Append(Script[j]);
                }
                else
                {
                    break;
                }
            }

            Signature s = IdentifyIdentifier(identifier.ToString());
            Tokens.Add(new Token()
            {
                Word = identifier.ToString(),
                Signature = s
            });
            return j - 1;
        }

        private Signature IdentifyIdentifier(string identifier)
        {
            if (identifier.IsPrimitiveType())
            {
                return Signature.TYPENAME;
            }

            switch (identifier)
            {
                case "if":
                    return Signature.KW_IF;
                case "else":
                    return Signature.KW_ELSE;
                case "while":
                    return Signature.KW_WHILE;
                case "switch":
                    return Signature.KW_SWITCH;
                case "case":
                    return Signature.KW_CASE;
                case "for":
                    return Signature.KW_FOR;
                case "in":
                    return Signature.KW_IN;
                case "is":
                    return Signature.KW_IS;
                case "do":
                    return Signature.KW_DO;

                case "return":
                    return Signature.KW_RETURN;
                case "continue":
                    return Signature.KW_CONTINUE;
                case "break":
                    return Signature.KW_BREAK;

                case "function":
                    return Signature.KW_FUNCTION_DECL;
                case "enum":
                    return Signature.KW_ENUM_DECL;

                case "true":
                    return Signature.KW_TRUE;
                case "false":
                    return Signature.KW_FALSE;

                case "World":
                    return Signature.CONST_WORLD;
                case "Self":
                    return Signature.CONST_SELF;
                case "Script":
                    return Signature.CONST_SCRIPT;
            }

            return Signature.IDENTIFIER;
        }

        private bool IsNextIndexValid(int i)
        {
            //if (i > Script.Length) return false;
            if ((i + 1) > Script.Length
                    || i > Script.Length)
            {
                return false;
            }

            return true;
        }

        private bool AreOpenCloseStatementsPaired()
        {
            bool validity = true;

            if (Script.Count(c => c == '{') != Script.Count(c => c == '}'))
            {
                validity = false;
                Logger.LogFatal("mismatched curly brackets group");
            }

            if (Script.Count(c => c == '(') != Script.Count(c => c == ')'))
            {
                validity = false;
                Logger.LogFatal("mismatched parenthesis group");
            }

            if (Script.Count(c => c == '[') != Script.Count(c => c == ']'))
            {
                validity = false;
                Logger.LogFatal("mismatched brackets group");
            }

            if ((Script.Count(c => c == '"') % 2) != 0)
            {
                validity = false;
                Logger.LogFatal("mismatched doublequote group");
            }

            return validity;
        }
    }
}

