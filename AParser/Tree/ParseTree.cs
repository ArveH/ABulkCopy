namespace AParser.Tree;

public class ParseTree : IParseTree
{
    private readonly INodeFactory _nodeFactory;
    private readonly ISqlTypes _sqlTypes;

    public ParseTree(
        INodeFactory nodeFactory,
        ISqlTypes sqlTypes)
    {
        _nodeFactory = nodeFactory;
        _sqlTypes = sqlTypes;
    }

    public INode CreateExpression(ITokenizer tokenizer)
    {
        switch (tokenizer.CurrentToken.Type)
        {
            case TokenType.LeftParenthesesToken:
                return CreateParentheses(tokenizer);
            case TokenType.NameToken:
                return CreateFunction(tokenizer);
            case TokenType.NumberToken:
                return CreateNumber(tokenizer);
            default:
                throw new AParserException(ErrorMessages.UnexpectedToken(tokenizer.CurrentToken.Type));
        }
    }

    public INode CreateFunction(ITokenizer tokenizer)
    {
        var functionName = tokenizer.CurrentTokenText.ToLower();
        switch (functionName)
        {
            case "convert":
                return CreateConvertFunction(tokenizer);
            default:
                throw new UnknownFunctionException(tokenizer.CurrentTokenText);
        }
    }

    public INode CreateConvertFunction(ITokenizer tokenizer)
    {
        var convertFunctionNode = _nodeFactory.Create(NodeType.ConvertFunctionNode);
        convertFunctionNode.Children.Add(CreateName(tokenizer));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(ParseLeftParentheses(tokenizer));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(CreateType(tokenizer));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(ParseComma(tokenizer));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(CreateExpression(tokenizer));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(ParseRightParentheses(tokenizer));

        return convertFunctionNode;
    }

    public INode CreateParentheses(ITokenizer tokenizer)
    {
        var parenthesesNode = _nodeFactory.Create(NodeType.ParenthesesNode);
        parenthesesNode.Children.Add(ParseLeftParentheses(tokenizer));

        tokenizer.GetNext();
        parenthesesNode.Children.Add(CreateExpression(tokenizer));

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

    public INode CreateNumber(ITokenizer tokenizer)
    {
        return ParseLeafNode(TokenType.NumberToken, tokenizer);
    }

    public INode ParseComma(ITokenizer tokenizer)
    {
        return ParseLeafNode(TokenType.CommaToken, tokenizer);
    }

    public INode CreateType(ITokenizer tokenizer)
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

    public INode CreateName(ITokenizer tokenizer)
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