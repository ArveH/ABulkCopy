namespace AParser.Exceptions;

public class TokenArgumentNullException : Exception
{
    public TokenArgumentNullException(string expected)
    : base(ErrorMessages.NullToken(expected))
    {
    }

    public TokenArgumentNullException(TokenName tokenName)
        : base(ErrorMessages.NullToken(tokenName.ToString()))
    {
    }
}