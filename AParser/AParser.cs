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
            case TokenName.SquareLeftParenthesesToken:
                expressionNode.Children!.Add(ParseName());
                break;
            case TokenName.NumberToken:
                expressionNode.Children!.Add(ParseNumber());
                break;
            case TokenName.LeftParenthesesToken:
                expressionNode.Children!.Add(ParseParentheses());
                break;
            default:
                throw new UnexpectedTokenException(NodeType.ExpressionNode, _currentToken.Name);
        }

        return expressionNode;
    }

    private INode ParseName()
    {
        if (_currentToken.Name == TokenName.SquareLeftParenthesesToken)
        {
            return ParseQuotedNameNode();
        }

        if (_currentToken.Name != TokenName.NameToken)
        {
            throw new UnexpectedTokenException(NodeType.NameLeafNode, _currentToken.Name);
        }

        return CreateLeafNode(NodeType.NameLeafNode, _currentToken);
    }

    private INode ParseQuotedNameNode()
    {
        if (_currentToken.Name != TokenName.SquareLeftParenthesesToken)
        {
            throw new UnexpectedTokenException(NodeType.SquareLeftParenthesesLeafNode, _currentToken.Name);
        }

        var quotedNameNode = CreateNode(NodeType.QuotedNameNode);
        quotedNameNode.Children!.Add(
            CreateLeafNode(NodeType.SquareLeftParenthesesLeafNode, _currentToken));
        GetNextToken();

        if (_currentToken.Name != TokenName.NameToken)
        {
            throw new UnexpectedTokenException(NodeType.NameLeafNode, _currentToken.Name);
        }

        quotedNameNode.Children!.Add(CreateLeafNode(NodeType.NameLeafNode, _currentToken));
        GetNextToken();

        if (_currentToken.Name != TokenName.SquareRightParenthesesToken)
        {
            throw new UnexpectedTokenException(NodeType.SquareRightParenthesesLeafNode, _currentToken.Name);
        }

        quotedNameNode.Children!.Add(
            CreateLeafNode(NodeType.SquareRightParenthesesLeafNode, _currentToken));

        return quotedNameNode;
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

        parenthesesNode.Children!.Add(CreateLeafNode(NodeType.LeftParenthesesLeafNode, _currentToken));
        parenthesesNode.Children.Add(ParseExpression());
        if (_currentToken.Name != TokenName.RightParenthesesToken)
        {
            throw new UnexpectedTokenException(NodeType.RightParenthesesLeafNode, _currentToken.Name);
        }

        parenthesesNode.Children!.Add(CreateLeafNode(NodeType.RightParenthesesLeafNode, _currentToken));

        return parenthesesNode;
    }

    private void GetNextToken()
    {
        _currentToken = _tokenizer.GetNext();
    }

    private INode CreateNode(NodeType nodeType)
    {
        return _nodeFactory.CreateNode(nodeType);
    }

    private INode CreateLeafNode(NodeType nodeType, IToken token)
    {
        return _nodeFactory.CreateNode(nodeType, token);
    }
}