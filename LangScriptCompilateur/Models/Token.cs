using System;
using System.Collections.Generic;
using System.Text;

namespace LangScriptCompilateur.Models
{
    public class Token
    {
        public string Word { get; set; }
        public Signature Signature { get; set; }
    }
    public enum Signature
    {
        //Non chars
        UNKNOWN,
        TYPENAME,
        STRINGLITTERAL,
        IDENTIFIER,

        END,
        PREPROCESSOR,
        I_CONST,
        F_CONST,
        LBRACKET,
        RBRACKET,
        LPAREN,
        RPAREN,
        COMMA,
        LBRACE,
        RBRACE,
        COLON,

        //OPERATORS
        OP_DOT,
        OP_PLUS,
        OP_PLUS_ASSIGN,
        OP_MINUS,
        OP_MINUS_ASSIGN,
        OP_MUL,
        OP_MUL_ASSIGN,
        OP_INCREMENT,
        OP_DECREMENT,
        OP_EQUALS,
        OP_NOTEQUALS,
        OP_GREATER_THAN_OR_EQUALS,
        OP_LESS_THAN_OR_EQUALS,
        OP_GREATER_THAN,
        OP_LESS_THAN,
        OP_NOT,
        OP_ASSIGN,
        OP_AND,
        OP_OR,

        //Keywords
        KW_IF,
        KW_ELSE,
        KW_WHILE,
        KW_SWITCH,
        KW_CASE,
        KW_FOR,
        KW_IN,
        KW_IS,
        KW_DO,

        KW_RETURN,
        KW_CONTINUE,
        KW_BREAK,

        KW_FUNCTION_DECL,
        KW_ENUM_DECL,
    }
}
