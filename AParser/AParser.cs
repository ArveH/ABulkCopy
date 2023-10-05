namespace AParser;

public class AParser : IAParser
{
    private readonly ISqlTypes _sqlTypes;

    public AParser(ISqlTypes sqlTypes)
    {
        _sqlTypes = sqlTypes;
    }

    public IEnumerable<IToken> ParseExpression(ITokenizer tokenizer, IParseTree parseTree)
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

    public IEnumerable<IToken> ParseFunction(ITokenizer tokenizer, IParseTree parseTree)
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

    public IEnumerable<IToken> ParseConvertFunction(ITokenizer tokenizer, IParseTree parseTree)
    {
        yield return tokenizer.CurrentToken;
        tokenizer.GetExpected(TokenType.LeftParenthesesToken);
        yield return tokenizer.CurrentToken;
        tokenizer.GetNext();

        foreach (var token in ParseType(tokenizer, parseTree))
        {
            yield return token;
        }

        tokenizer.GetExpected(TokenType.CommaToken);
        yield return tokenizer.CurrentToken;
        tokenizer.GetNext();

        foreach (var token in ParseExpression(tokenizer, parseTree))
        {
            yield return token;
        }

        tokenizer.GetExpected(TokenType.RightParenthesesToken);
        yield return tokenizer.CurrentToken;
    }

    public IEnumerable<IToken> ParseParentheses(ITokenizer tokenizer, IParseTree parseTree)
    {
        if (tokenizer.CurrentToken.Type != TokenType.LeftParenthesesToken)
        {
            throw new UnexpectedTokenException(TokenType.LeftParenthesesToken, tokenizer.CurrentToken.Type);
        }
        yield return tokenizer.CurrentToken;
        tokenizer.GetNext();

        foreach (var token in ParseExpression(tokenizer, parseTree))
        {
            yield return token;
        }

        tokenizer.GetExpected(TokenType.RightParenthesesToken);
        yield return tokenizer.CurrentToken;
    }

    public IEnumerable<IToken> ParseNumber(ITokenizer tokenizer, IParseTree parseTree)
    {
        if (tokenizer.CurrentToken.Type != TokenType.NumberToken)
        {
            throw new UnexpectedTokenException(TokenType.NumberToken, tokenizer.CurrentToken.Type);
        }
        yield return tokenizer.CurrentToken;
    }

    public IEnumerable<IToken> ParseType(ITokenizer tokenizer, IParseTree parseTree)
    {
        var type = tokenizer.CurrentToken.Type == TokenType.QuotedNameToken
            ? tokenizer.CurrentTokenText[1..^1]
                : tokenizer.CurrentTokenText;
        if (!_sqlTypes.Exist(type))
        {
            throw new UnknownSqlTypeException(type);
        }
        yield return tokenizer.CurrentToken;
    }

    public IEnumerable<IToken> ParseName(ITokenizer tokenizer, IParseTree parseTree)
    {
        if (tokenizer.CurrentToken.Type != TokenType.NameToken)
        {
            throw new UnexpectedTokenException(TokenType.NameToken, tokenizer.CurrentToken.Type);
        }
        yield return tokenizer.CurrentToken;
    }
}