namespace ABulkCopy.Common.Exceptions;

public class NotInitializedException: Exception
{
    public NotInitializedException(string message)
        : base(message)
    {
    }
}
