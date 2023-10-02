namespace AParser.ParseTree.Nodes;

public class FunctionNode : INode
{
    private readonly INodeFactory _nodeFactory;

    public FunctionNode(INodeFactory nodeFactory)
    {
        _nodeFactory = nodeFactory;
    }

    public NodeType Type => NodeType.FunctionNode;

    public bool IsLeafNode => false;

    public void Create(ITokenizer tokenizer)
    {
        var currentToken = tokenizer.GetNext();
        if (currentToken.Name != TokenName.NameToken)
        {
            throw new UnexpectedTokenException(TokenName.NameToken, currentToken);
        }

        switch (tokenizer.GetSpelling(currentToken))
        {
            case var s when s.Equals("convert", StringComparison.InvariantCultureIgnoreCase):
                var functionNameNode = _nodeFactory.CreateNode(NodeType.NameLeafNode, tokenizer);
                Children.Add(_nodeFactory.CreateNode(NodeType.ConvertFunctionNode, tokenizer));
                break;
        }

        throw new ArgumentOutOfRangeException(
            nameof(currentToken),
            $"Illegal function {tokenizer.GetSpelling(currentToken)}");
    }

    public List<INode> Children { get; } = new();
}