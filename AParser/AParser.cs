namespace AParser;

public class AParser : IAParser
{
    private readonly ISqlFunctions _sqlFunctions;
    private readonly ISqlTypes _sqlTypes;

    public AParser(
        ISqlFunctions sqlFunctions,
        ISqlTypes sqlTypes)
    {
        _sqlFunctions = sqlFunctions;
        _sqlTypes = sqlTypes;
    }

    public void ParseExpression(ITokenizer tokenizer, IParseTree parseTree)
    {
        switch (tokenizer.CurrentToken.Name)
        {
            case TokenName.NameToken:
            case TokenName.SquareLeftParenthesesToken:
                ParseName(tokenizer, parseTree);
                if (IsFunction(tokenizer.CurrentTokenText))
                {
                    ParseFunction(tokenizer, parseTree);
                }
                break;
            case TokenName.NumberToken:
                ParseNumber(tokenizer, parseTree);
                break;
            case TokenName.LeftParenthesesToken:
                ParseParentheses(tokenizer, parseTree);
                break;
            default:
                throw new AParserException(ErrorMessages.UnexpectedToken(tokenizer.CurrentToken.Name));
        }
    }

    public void ParseFunction(ITokenizer tokenizer, IParseTree parseTree)
    {
        switch (tokenizer.CurrentTokenText)
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
        tokenizer.GetExpected(TokenName.LeftParenthesesToken);
        tokenizer.GetNext();

        ParseType(tokenizer, parseTree);

        tokenizer.GetExpected(TokenName.CommaToken);
        tokenizer.GetNext();

        ParseExpression(tokenizer, parseTree);

        tokenizer.GetExpected(TokenName.RightParenthesesToken);
    }

    public void ParseParentheses(ITokenizer tokenizer, IParseTree parseTree)
    {
        throw new NotImplementedException();
    }

    public void ParseNumber(ITokenizer tokenizer, IParseTree parseTree)
    {
        throw new NotImplementedException();
    }

    public void ParseQuotedName(ITokenizer tokenizer, IParseTree parseTree)
    {
        throw new NotImplementedException();
    }

    public void ParseType(ITokenizer tokenizer, IParseTree parseTree)
    {
        throw new NotImplementedException();
    }

    public void ParseName(ITokenizer tokenizer, IParseTree parseTree)
    {
        var isQuoted = false;
        if (tokenizer.CurrentToken.Name == TokenName.SquareLeftParenthesesToken)
        {
            isQuoted = true;
            tokenizer.GetNext();
        }

        if (tokenizer.CurrentToken.Name != TokenName.NameToken)
        {
            throw new UnexpectedTokenException(
                TokenName.NameToken, tokenizer.CurrentToken.Name);
        }

        if (isQuoted)
        {
            tokenizer.GetExpected(TokenName.SquareRightParenthesesToken);
        }
    }

    private bool IsFunction(string name)
    {
        return _sqlFunctions.Exist(name);
    }
}