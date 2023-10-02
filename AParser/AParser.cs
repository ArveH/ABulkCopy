namespace AParser;

public class AParser : IAParser
{
    private readonly INodeFactory _nodeFactory;
    private readonly ITokenizer _tokenizer;
    private IToken _currentToken = UndefinedToken.Instance;

    public AParser(
        INodeFactory nodeFactory,
        ITokenizer tokenizer)
    {
        _nodeFactory = nodeFactory;
        _tokenizer = tokenizer;
    }

    public INode Parse(string sql)
    {
        if (string.IsNullOrWhiteSpace(sql))
        {
            throw new AParserException(ErrorMessages.EmptySql);
        }

        _tokenizer.Initialize(sql);
        _currentToken = _tokenizer.GetNext();
        var rootNode = CreateNode(NodeType.ExpressionNode);

        switch (_currentToken.Name)
        {
            case TokenName.LeftParenthesesToken:
                rootNode.Children.Add(ParseParentheses());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return rootNode;
    }

    private INode ParseExpression()
    {
        throw new NotImplementedException();
    }

    private INode ParseParentheses()
    {
        var parenthesesNode = CreateNode(NodeType.ParenthesesNode);
        if (_currentToken.Name != TokenName.LeftParenthesesToken)
        {
            throw new UnexpectedTokenException(NodeType.LeftParenthesesLeafNode, _currentToken.Name);
        }
        parenthesesNode.Children.Add(CreateNode(NodeType.LeftParenthesesLeafNode));
        parenthesesNode.Children.Add(ParseExpression());
        if (_currentToken.Name != TokenName.RightParenthesesToken)
        {
            throw new UnexpectedTokenException(NodeType.RightParenthesesLeafNode, _currentToken.Name);
        }

        return parenthesesNode;
    }

    private INode CreateNode(NodeType nodeType)
    {
        return _nodeFactory.CreateNode(nodeType);
    }
}