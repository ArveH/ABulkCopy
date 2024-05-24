namespace CrossRDBMS.Tests.Helpers;

public static class ParamHelper
{
    public static string[] GetOutMss(
        string connectionString,
        string? folder = null,
        string? logFile = null,
        string? addQuotes = null,
        string? schemaFilter = null,
        string? searchFilter = null,
        string? emptyString = null)
    {
        return GetArgs(
            CopyDirection.Out.ToString(),
            Rdbms.Mss.ToString(),
            connectionString,
            folder, logFile, addQuotes, schemaFilter, searchFilter, emptyString);
    }

    public static string[] GetInPg(
        string connectionString,
        string? folder = null,
        string? logFile = null,
        string? addQuotes = null,
        string? schemaFilter = null,
        string? searchFilter = null,
        string? emptyString = null)
    {
        return GetArgs(
            CopyDirection.In.ToString(),
            Rdbms.Pg.ToString(),
            connectionString,
            folder, logFile, addQuotes, schemaFilter, searchFilter, emptyString);
    }

    public static string[] GetArgs(
        string direction,
        string rdbms,
        string connectionString,
        string? folder= null,
        string? logFile= null,
        string? addQuotes= null,
        string? schemaFilter= null,
        string? searchFilter= null,
        string? emptyString= null)
    {
        var args = new List<string>
        {
            "-d",
            direction,
            "-r",
            rdbms,
            "-c",
            connectionString,
            "-f",
            folder ?? ".",
            "-l",
            logFile ?? Path.Combine(".", "local_out.log")
        };
        if (addQuotes != null)
        {
            args.Add("-q");
            args.Add(addQuotes);
        }
        if (schemaFilter != null)
        {
            args.Add("--schema-filter");
            args.Add(schemaFilter);
        }
        if (searchFilter != null)
        {
            args.Add("-s");
            args.Add(searchFilter);
        }
        if (emptyString != null)
        {
            args.Add("--empty-string");
            args.Add(emptyString);
        }

        return args.ToArray();
    }
}