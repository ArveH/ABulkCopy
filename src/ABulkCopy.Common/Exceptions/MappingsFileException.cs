namespace ABulkCopy.Common.Exceptions;

public class MappingsFileException: Exception
{
    public MappingsFileException(string message)
        : base(message)
    {
    }

    public MappingsFileException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
