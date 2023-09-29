namespace AParser.Exceptions;

public class CreateNodeException : Exception
{
    public TokenName ExpectedToken { get; }
    public IToken ActualToken { get; }

    public CreateNodeException(TokenName expectedToken, IToken actualToken)
    {
        ExpectedToken = expectedToken;
        ActualToken = actualToken;
    }
}