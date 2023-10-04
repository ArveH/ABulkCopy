namespace AParser;

public class SqlFunctions : ISqlFunctions
{
    private static readonly HashSet<string> FunctionNames = new(StringComparer.InvariantCultureIgnoreCase)
    {
        "convert"
    };

    public bool Exist(string name)
    {
        return FunctionNames.Contains(name);
    }
}