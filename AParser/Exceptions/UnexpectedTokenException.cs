namespace AParser.Exceptions;

public class UnexpectedTokenException : Exception
{
    public TokenName? Expected { get; }
    public TokenName Current { get; }

    public UnexpectedTokenException(TokenName expected, TokenName current)
    : base(ErrorMessages.UnexpectedToken(expected, current))
    {
        Expected = expected;
        Current = current;
    }

    public UnexpectedTokenException(TokenName current)
        : base(ErrorMessages.UnexpectedToken(current))
    {
        Current = current;
    }
}