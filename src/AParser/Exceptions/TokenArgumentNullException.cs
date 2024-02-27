namespace AParser.Exceptions;

public class TokenArgumentNullException : Exception
{
    public TokenArgumentNullException(string expected)
    : base(ErrorMessages.NullToken(expected))
    {
    }

    public TokenArgumentNullException(TokenType tokenType)
        : base(ErrorMessages.NullToken(tokenType.ToString()))
    {
    }
}