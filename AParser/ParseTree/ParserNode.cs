namespace AParser.ParseTree;

public abstract class ParserNode : IParserNode
{
    public NodeType Type { get; init; }
    public bool IsLeafNode => Token == null;
    public abstract void Create(ITokenizer tokenizer);
    public virtual IToken? Token => null;
    public List<IParserNode> Children { get; } = new();
}