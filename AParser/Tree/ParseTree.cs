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
            case TokenType.StringToken:
                return CreateString(tokenizer);
            case TokenType.NStringToken:
                return CreateNString(tokenizer);
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
        convertFunctionNode.Children.Add(CreateLeafNode(TokenType.LeftParenthesesToken, tokenizer));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(CreateType(tokenizer));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(CreateLeafNode(TokenType.CommaToken, tokenizer));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(CreateExpression(tokenizer));

        tokenizer.GetNext();
        if (tokenizer.CurrentToken.Type == TokenType.CommaToken)
        {
            convertFunctionNode.Children.Add(CreateLeafNode(TokenType.CommaToken, tokenizer));

            tokenizer.GetNext();
            convertFunctionNode.Children.Add(CreateExpression(tokenizer));

            tokenizer.GetNext();
        }
        convertFunctionNode.Children.Add(CreateLeafNode(TokenType.RightParenthesesToken, tokenizer));

        return convertFunctionNode;
    }

    public INode CreateParentheses(ITokenizer tokenizer)
    {
        var parenthesesNode = _nodeFactory.Create(NodeType.ParenthesesNode);
        parenthesesNode.Children.Add(CreateLeafNode(TokenType.LeftParenthesesToken, tokenizer));

        tokenizer.GetNext();
        parenthesesNode.Children.Add(CreateExpression(tokenizer));

        tokenizer.GetNext();
        parenthesesNode.Children.Add(CreateLeafNode(TokenType.RightParenthesesToken, tokenizer));

        return parenthesesNode;
    }

    public INode CreateNumber(ITokenizer tokenizer)
    {
        return CreateLeafNode(TokenType.NumberToken, tokenizer);
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
        return CreateLeafNode(TokenType.NameToken, tokenizer);
    }

    public INode CreateString(ITokenizer tokenizer)
    {
        return CreateLeafNode(tokenizer.CurrentToken.Type, tokenizer);
    }

    public INode CreateNString(ITokenizer tokenizer)
    {
        return CreateLeafNode(tokenizer.CurrentToken.Type, tokenizer);
    }

    private INode CreateLeafNode(TokenType type, ITokenizer tokenizer)
    {
        if (tokenizer.CurrentToken.Type != type)
        {
            throw new UnexpectedTokenException(type, tokenizer.CurrentToken.Type);
        }
        return CreateLeafNode(tokenizer.CurrentToken);
    }

    private INode CreateLeafNode(IToken token, NodeType? type = null)
    {
        var node = _nodeFactory.Create(type ?? token.Type.ToNodeType());
        node.Tokens.Add(token);
        return node;
    }
}