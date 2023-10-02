namespace AParser.Exceptions;

public class UnexpectedTokenException : Exception
{
    public TokenName ExpectedToken { get; }
    public IToken ActualToken { get; }

    public UnexpectedTokenException(TokenName expectedToken, IToken actualToken)
    {
        ExpectedToken = expectedToken;
        ActualToken = actualToken;
    }
}