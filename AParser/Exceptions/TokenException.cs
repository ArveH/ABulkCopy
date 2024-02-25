namespace AParser.Exceptions;

public class TokenException : Exception
{
    public TokenException(string errorMessage) : base(errorMessage)
    {
    }
}