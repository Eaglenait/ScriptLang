namespace LangScriptCompilateur.Models
{
    public class ASTSubBlock
    {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int Depth { get; set; }
        public Signature BlockType { get; set; }

    }
}
