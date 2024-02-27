namespace AParser.Exceptions;

public class UnclosedException : Exception
{
    public UnclosedException(char expectedChar)
        : base(ErrorMessages.Unclosed(expectedChar))
    {
        
    }
}