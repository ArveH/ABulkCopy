namespace ABulkCopy.Common.Extensions;

public static class ExceptionExtensions
{
    public static string FlattenMessages(this Exception? ex)
    {
        var sb = new StringBuilder();
        while (ex != null)
        {
            sb.AppendLine(ex.Message);
            ex = ex.InnerException;
        }

        return sb.ToString();
    }
}