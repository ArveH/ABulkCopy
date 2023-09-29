namespace AParser.ParseTree.Nodes;

public class ExpressionNode : INode
{
    public NodeType Type => NodeType.ExpressionNode;

    public bool IsLeafNode => false;

    public void Create(ITokenizer tokenizer)
    {
        var token = tokenizer.GetNext();

        throw new ArgumentOutOfRangeException(nameof(TokenName), $"Unsupported Token {token.Name} for {nameof(ExpressionNode)}");
    }

    public List<INode> Children { get; } = new();
}