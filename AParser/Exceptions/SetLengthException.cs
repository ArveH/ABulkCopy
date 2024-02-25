namespace AParser.Exceptions;

public class SetLengthException : Exception
{
    public SetLengthException(TokenType tokenType) : 
        base($"Can't set length for token {tokenType}")
    { }
}