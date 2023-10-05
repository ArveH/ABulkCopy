using AParser.Tree;

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

    public INode ParseExpression(ITokenizer tokenizer, IParseTree parseTree)
    {
        switch (tokenizer.CurrentToken.Type)
        {
            case TokenType.LeftParenthesesToken:
                return ParseParentheses(tokenizer, parseTree);
            case TokenType.NameToken:
                return ParseFunction(tokenizer, parseTree);
            case TokenType.NumberToken:
                return ParseNumber(tokenizer, parseTree);
            default:
                throw new AParserException(ErrorMessages.UnexpectedToken(tokenizer.CurrentToken.Type));
        }
    }

    public INode ParseFunction(ITokenizer tokenizer, IParseTree parseTree)
    {
        var functionName = tokenizer.CurrentTokenText.ToLower();
        switch (functionName)
        {
            case "convert":
                return ParseConvertFunction(tokenizer, parseTree);
            default:
                throw new UnknownFunctionException(tokenizer.CurrentTokenText);
        }
    }

    public INode ParseConvertFunction(ITokenizer tokenizer, IParseTree parseTree)
    {
        var convertFunctionNode = _nodeFactory.Create(NodeType.ConvertFunctionNode);
        convertFunctionNode.Children.Add(ParseName(tokenizer, parseTree));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(ParseLeftParentheses(tokenizer, parseTree));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(ParseType(tokenizer, parseTree));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(ParseComma(tokenizer, parseTree));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(ParseExpression(tokenizer, parseTree));

        tokenizer.GetNext();
        convertFunctionNode.Children.Add(ParseRightParentheses(tokenizer, parseTree));

        return convertFunctionNode;
    }

    public INode ParseParentheses(ITokenizer tokenizer, IParseTree parseTree)
    {
        var parenthesesNode = _nodeFactory.Create(NodeType.ParenthesesNode);
        parenthesesNode.Children.Add(ParseLeftParentheses(tokenizer, parseTree));

        tokenizer.GetNext();
        parenthesesNode.Children.Add(ParseExpression(tokenizer, parseTree));

        tokenizer.GetNext();
        parenthesesNode.Children.Add(ParseRightParentheses(tokenizer, parseTree));

        return parenthesesNode;
    }

    public INode ParseLeftParentheses(ITokenizer tokenizer, IParseTree parseTree)
    {
        return ParseLeafNode(TokenType.LeftParenthesesToken, tokenizer, parseTree);
    }

    public INode ParseRightParentheses(ITokenizer tokenizer, IParseTree parseTree)
    {
        return ParseLeafNode(TokenType.RightParenthesesToken, tokenizer, parseTree);
    }

    public INode ParseNumber(ITokenizer tokenizer, IParseTree parseTree)
    {
        return ParseLeafNode(TokenType.NumberToken, tokenizer, parseTree);
    }

    public INode ParseComma(ITokenizer tokenizer, IParseTree parseTree)
    {
        return ParseLeafNode(TokenType.CommaToken, tokenizer, parseTree);
    }

    public INode ParseType(ITokenizer tokenizer, IParseTree parseTree)
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

    public INode ParseName(ITokenizer tokenizer, IParseTree parseTree)
    {
        return ParseLeafNode(TokenType.NameToken, tokenizer, parseTree);
    }

    private INode ParseLeafNode(TokenType type, ITokenizer tokenizer, IParseTree parseTree)
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