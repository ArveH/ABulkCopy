namespace AParser;

public class AParser : IAParser
{
    private readonly ISqlTypes _sqlTypes;

    public AParser(ISqlTypes sqlTypes)
    {
        _sqlTypes = sqlTypes;
    }

    public void ParseExpression(ITokenizer tokenizer, IParseTree parseTree)
    {
        switch (tokenizer.CurrentToken.Type)
        {
            case TokenType.LeftParenthesesToken:
                ParseParentheses(tokenizer, parseTree);
                break;
            case TokenType.NameToken:
                ParseFunction(tokenizer, parseTree);
                break;
            case TokenType.NumberToken:
                ParseNumber(tokenizer, parseTree);
                break;
            default:
                throw new AParserException(ErrorMessages.UnexpectedToken(tokenizer.CurrentToken.Type));
        }
    }

    public void ParseFunction(ITokenizer tokenizer, IParseTree parseTree)
    {
        var functionName = tokenizer.CurrentTokenText.ToLower();
        switch (functionName)
        {
            case "convert":
                ParseConvertFunction(tokenizer, parseTree);
                return;
            default:
                throw new UnknownFunctionException(tokenizer.CurrentTokenText);
        }
    }

    public void ParseConvertFunction(ITokenizer tokenizer, IParseTree parseTree)
    {
        tokenizer.GetExpected(TokenType.LeftParenthesesToken);
        tokenizer.GetNext();

        ParseType(tokenizer, parseTree);

        tokenizer.GetExpected(TokenType.CommaToken);
        tokenizer.GetNext();

        ParseExpression(tokenizer, parseTree);

        tokenizer.GetExpected(TokenType.RightParenthesesToken);
    }

    public void ParseParentheses(ITokenizer tokenizer, IParseTree parseTree)
    {
        if (tokenizer.CurrentToken.Type != TokenType.LeftParenthesesToken)
        {
            throw new UnexpectedTokenException(TokenType.LeftParenthesesToken, tokenizer.CurrentToken.Type);
        }
        tokenizer.GetNext();

        ParseExpression(tokenizer, parseTree);

        tokenizer.GetExpected(TokenType.RightParenthesesToken);
    }

    public void ParseNumber(ITokenizer tokenizer, IParseTree parseTree)
    {
        if (tokenizer.CurrentToken.Type != TokenType.NumberToken)
        {
            throw new UnexpectedTokenException(TokenType.NumberToken, tokenizer.CurrentToken.Type);
        }
    }

    public void ParseType(ITokenizer tokenizer, IParseTree parseTree)
    {
        var type = tokenizer.CurrentToken.Type == TokenType.QuotedNameToken
            ? tokenizer.CurrentTokenText[1..^1]
                : tokenizer.CurrentTokenText;
        if (!_sqlTypes.Exist(type))
        {
            throw new UnknownSqlTypeException(type);
        }
    }

    public void ParseName(ITokenizer tokenizer, IParseTree parseTree)
    {
        if (tokenizer.CurrentToken.Type != TokenType.NameToken)
        {
            throw new UnexpectedTokenException(TokenType.NameToken, tokenizer.CurrentToken.Type);
        }
    }
}