using LangScriptCompilateur.Models;

namespace LangScriptCompilateur
{
    public interface IParseRule
    {
        SyntaxNode Execute();
    }
}
