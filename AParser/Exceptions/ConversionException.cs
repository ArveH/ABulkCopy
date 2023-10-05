namespace AParser.Exceptions;

public class ConversionException : Exception
{
    public ConversionException(string from, string toTypeName)
        : base(ErrorMessages.Conversion(from, toTypeName))
    {
        
    }
}