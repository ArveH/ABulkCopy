namespace AParser;

public class AParser : IAParser
{
    private readonly INodeFactory _nodeFactory;
    private readonly ITokenizer _tokenizer;
    private IToken _currentToken = UndefinedToken.Instance;

    private static readonly HashSet<string> FunctionNames = new(StringComparer.InvariantCultureIgnoreCase)
    {
        "convert"
    };

    private static readonly HashSet<string> SqlTypes = new(StringComparer.InvariantCultureIgnoreCase)
    {
        "bit"
    };

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
        GetNextToken();
        return ParseExpression();
    }

    private INode ParseExpression()
    {
        var expressionNode = CreateNode(NodeType.ExpressionNode);

        switch (_currentToken.Name)
        {
            case TokenName.NameToken:
                var name = GetCurrentTokenSpelling();
                expressionNode.Children!.Add(IsFunction(name)
                    ? ParseFunction(name)
                    : CreateLeafNode(NodeType.NameLeafNode, _currentToken));
                break;
            case TokenName.SquareLeftParenthesesToken:
                expressionNode.Children!.Add(ParseQuotedName());
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

        GetNextToken();
        return expressionNode;
    }

    private INode ParseFunction(string name)
    {
        if (name.Equals("convert", StringComparison.InvariantCultureIgnoreCase))
        {
            return ParseConvertFunction();
        }

        throw new UnknownFunctionException(name);
    }

    private INode ParseConvertFunction()
    {
        var functionNode = CreateNode(NodeType.ConvertFunctionNode);
        GetNextToken();
        if (_currentToken.Name != TokenName.LeftParenthesesToken)
        {
            throw new UnexpectedTokenException(NodeType.LeftParenthesesLeafNode, _currentToken.Name);
        }
        functionNode.Children!.Add(CreateLeafNode(NodeType.LeftParenthesesLeafNode, _currentToken));
        GetNextToken();
        functionNode.Children.Add(ParseName());
        if (!IsSqlType(functionNode.Children.Last()))
        {
            throw new UnknownSqlTypeException(GetCurrentTokenSpelling());
        }
        GetNextToken();
        if (_currentToken.Name != TokenName.CommaToken)
        {
            throw new UnexpectedTokenException(NodeType.CommaLeafNode, _currentToken.Name);
        }
        functionNode.Children.Add(CreateLeafNode(NodeType.CommaLeafNode, _currentToken));
        GetNextToken();
        functionNode.Children.Add(ParseExpression());
        GetNextToken();
        if (_currentToken.Name != TokenName.RightParenthesesToken)
        {
            throw new UnexpectedTokenException(NodeType.RightParenthesesLeafNode, _currentToken.Name);
        }
        functionNode.Children.Add(CreateLeafNode(NodeType.RightParenthesesLeafNode, _currentToken));
        GetNextToken();

        return functionNode;
    }

    private INode ParseName()
    {
        if (_currentToken.Name == TokenName.SquareLeftParenthesesToken)
        {
            return ParseQuotedName();
        }

        if (_currentToken.Name != TokenName.NameToken)
        {
            throw new UnexpectedTokenException(NodeType.NameLeafNode, _currentToken.Name);
        }

        return CreateLeafNode(NodeType.NameLeafNode, _currentToken);
    }

    private INode ParseQuotedName()
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
        GetNextToken();

        return quotedNameNode;
    }

    private INode ParseNumber()
    {
        if (_currentToken.Name != TokenName.NumberToken)
        {
            throw new UnexpectedTokenException(NodeType.NumberLeafNode, _currentToken.Name);
        }

        return CreateLeafNode(NodeType.NumberLeafNode, _currentToken);
    }

    private INode ParseParentheses()
    {
        var parenthesesNode = CreateNode(NodeType.ParenthesesNode);
        if (_currentToken.Name != TokenName.LeftParenthesesToken)
        {
            throw new UnexpectedTokenException(NodeType.LeftParenthesesLeafNode, _currentToken.Name);
        }

        parenthesesNode.Children!.Add(CreateLeafNode(NodeType.LeftParenthesesLeafNode, _currentToken));
        GetNextToken();
        parenthesesNode.Children.Add(ParseExpression());
        if (_currentToken.Name != TokenName.RightParenthesesToken)
        {
            throw new UnexpectedTokenException(NodeType.RightParenthesesLeafNode, _currentToken.Name);
        }

        parenthesesNode.Children!.Add(CreateLeafNode(NodeType.RightParenthesesLeafNode, _currentToken));

        return parenthesesNode;
    }

    private string GetCurrentTokenSpelling()
    {
        return _tokenizer.GetSpelling(_currentToken).ToString();
    }

    private bool IsFunction(string name)
    {
        return FunctionNames.Contains(name);
    }

    private bool IsSqlType(INode node)
    {
        if (node.IsLeafNode)
            return SqlTypes.Contains(node.Token!.Name.ToString());

        if (node.Children!.Count == 3 && node.Children[1].IsLeafNode)
            return SqlTypes.Contains(_tokenizer.GetSpelling(node.Children[1].Token!).ToString());

        return false;
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