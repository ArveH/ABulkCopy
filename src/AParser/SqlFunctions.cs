namespace AParser;

public class SqlFunctions
{
    private static readonly HashSet<string> FunctionNames = new(StringComparer.InvariantCultureIgnoreCase)
    {
        Convert,
        GetDate,
        NewId
    };

    public const string Convert = "convert";
    public const string GetDate = "getdate";
    public const string NewId = "newid";
}