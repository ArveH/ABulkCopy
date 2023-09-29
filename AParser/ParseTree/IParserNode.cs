namespace AParser.ParseTree;

public interface IParserNode
{
    NodeType Type { get; init; }
    bool IsLeafNode { get; }
    IToken? Token { get; }
    List<IParserNode> Children { get; }
    void Create(ITokenizer tokenizer);
}