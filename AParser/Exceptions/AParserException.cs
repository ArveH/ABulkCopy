namespace AParser.Exceptions;

public class AParserException : Exception
{
    public AParserException(string errorMessage)
        :base(errorMessage)
    {
        
    }
}