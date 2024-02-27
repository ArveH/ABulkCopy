namespace AParser.Exceptions;

public class UnexpectedTokenException : Exception
{
    public TokenType? Expected { get; }
    public TokenType Current { get; }

    public UnexpectedTokenException(TokenType expected, TokenType current)
    : base(ErrorMessages.UnexpectedToken(expected, current))
    {
        Expected = expected;
        Current = current;
    }

    public UnexpectedTokenException(TokenType current)
        : base(ErrorMessages.UnexpectedToken(current))
    {
        Current = current;
    }
}