namespace AParser;

public class AParser : IAParser
{
    private readonly INodeFactory _nodeFactory;
    private readonly ISqlTypes _sqlTypes;

    public AParser(
        INodeFactory nodeFactory,
        ISqlTypes sqlTypes)
    {
        _nodeFactory = nodeFactory;
        _sqlTypes = sqlTypes;
    }

    public INode ParseExpression(ITokenizer tokenizer)
    {
        switch (tokenizer.CurrentToken.Type)
        {
            case TokenType.LeftParenthesesToken:
                return ParseParentheses(tokenizer);
            case TokenType.NameToken:
                return ParseFunction(tokenizer);
            case TokenType.NumberToken:
                return ParseNumber(tokenizer);
            default:
                throw new AParserException(ErrorMessages.UnexpectedToken(tokenizer.CurrentToken.Type));
        }
    }

    public INode ParseFunction(ITokenizer tokenizer)
    {
        var functionName = tokenizer.CurrentTokenText.ToLower();
        switch (functionName)
        {
            case "convert":
                return ParseConvertFunction(tokenizer);
            default:
                throw new UnknownFunctionException(tokenizer.CurrentTokenText);
        }
    }

    public INode ParseConvertFunction(ITokenizer tokenizer)
    {
        var convertFunctionNode = _nodeFactory.Create(NodeType.ConvertFunctionNode);
        convertFunctionNode.Children.Add(ParseName(tokenizer));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(ParseLeftParentheses(tokenizer));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(ParseType(tokenizer));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(ParseComma(tokenizer));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(ParseExpression(tokenizer));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(ParseRightParentheses(tokenizer));

        return convertFunctionNode;
    }

    public INode ParseParentheses(ITokenizer tokenizer)
    {
        var parenthesesNode = _nodeFactory.Create(NodeType.ParenthesesNode);
        parenthesesNode.Children.Add(ParseLeftParentheses(tokenizer));

        tokenizer.GetNext();
        parenthesesNode.Children.Add(ParseExpression(tokenizer));

        tokenizer.GetNext();
        parenthesesNode.Children.Add(ParseRightParentheses(tokenizer));

        return parenthesesNode;
    }

    public INode ParseLeftParentheses(ITokenizer tokenizer)
    {
        return ParseLeafNode(TokenType.LeftParenthesesToken, tokenizer);
    }

    public INode ParseRightParentheses(ITokenizer tokenizer)
    {
        return ParseLeafNode(TokenType.RightParenthesesToken, tokenizer);
    }

    public INode ParseNumber(ITokenizer tokenizer)
    {
        return ParseLeafNode(TokenType.NumberToken, tokenizer);
    }

    public INode ParseComma(ITokenizer tokenizer)
    {
        return ParseLeafNode(TokenType.CommaToken, tokenizer);
    }

    public INode ParseType(ITokenizer tokenizer)
    {
        var type = tokenizer.CurrentToken.Type == TokenType.QuotedNameToken
            ? tokenizer.CurrentTokenText[1..^1]
                : tokenizer.CurrentTokenText;
        if (!_sqlTypes.Exist(type))
        {
            throw new UnknownSqlTypeException(type);
        }
        return CreateLeafNode(tokenizer.CurrentToken, NodeType.TypeNode);
    }

    public INode ParseName(ITokenizer tokenizer)
    {
        return ParseLeafNode(TokenType.NameToken, tokenizer);
    }

    private INode ParseLeafNode(TokenType type, ITokenizer tokenizer)
    {
        if (tokenizer.CurrentToken.Type != type)
        {
            throw new UnexpectedTokenException(type, tokenizer.CurrentToken.Type);
        }
        return CreateLeafNode(tokenizer.CurrentToken);
    }

    private INode CreateLeafNode(IToken token, NodeType? nodeType = null)
    {
        var node = _nodeFactory.Create(nodeType ?? token.Type.ToNodeType());
        node.Tokens.Add(token);
        return node;
    }
}