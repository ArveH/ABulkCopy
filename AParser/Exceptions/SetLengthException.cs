namespace AParser.Exceptions;

public class SetLengthException : Exception
{
    public SetLengthException(TokenName tokenName) : 
        base($"Can't set length for token {tokenName}")
    { }
}