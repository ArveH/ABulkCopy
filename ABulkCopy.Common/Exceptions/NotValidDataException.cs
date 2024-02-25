namespace ABulkCopy.Common.Exceptions;

public class NotValidDataException: Exception
{
    public NotValidDataException()
    {
    }

    public NotValidDataException(string message)
        : base(message)
    {
    }
}
