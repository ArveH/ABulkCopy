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
        return ParseExpression();
    }

    private INode ParseExpression()
    {
        var expressionNode = CreateNode(NodeType.ExpressionNode);
        GetNextToken();

        switch (_currentToken.Name)
        {
            case TokenName.NameToken:
                expressionNode.Children!.Add(ParseName());
                break;
            case TokenName.NumberToken:
                expressionNode.Children!.Add(ParseNumber());
                break;
            case TokenName.LeftParenthesesToken:
                expressionNode.Children!.Add(ParseParentheses());
                break;
            case TokenName.SquareLeftParenthesesToken:
                expressionNode.Children!.Add(ParseSquareParentheses());
                break;
            default:
                throw new UnexpectedTokenException(NodeType.ExpressionNode, _currentToken.Name);
        }

        return expressionNode;
    }

    private INode ParseName()
    {
        throw new NotImplementedException();
    }

    private INode ParseNumber()
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
        parenthesesNode.Children!.Add(CreateNode(NodeType.LeftParenthesesLeafNode));
        parenthesesNode.Children.Add(ParseExpression());
        if (_currentToken.Name != TokenName.RightParenthesesToken)
        {
            throw new UnexpectedTokenException(NodeType.RightParenthesesLeafNode, _currentToken.Name);
        }

        return parenthesesNode;
    }

    private INode ParseSquareParentheses()
    {
        throw new NotImplementedException();
    }

    private void GetNextToken()
    {
        _currentToken = _tokenizer.GetNext();
    }

    private INode CreateNode(NodeType nodeType)
    {
        return _nodeFactory.CreateNode(nodeType);
    }
}