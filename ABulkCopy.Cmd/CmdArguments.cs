namespace ABulkCopy.Cmd;

public class CmdArguments
{
    [Option('d', "direction", Required = true, HelpText = "Copy direction. \"In\" to a database or \"Out\" from a database.")]
    public required CopyDirection Direction { get; set; }

    [Option('c', "connection-string", HelpText = "The connection string")]
    public string? ConnectionString { get; set; }

    [Option('r', "rdbms", Required = true, HelpText = "Database Management System. \"Pg\" or \"Mss\".")]
    public required Rdbms Rdbms { get; set; }

    [Option('f', "folder", Required = false, HelpText = "The source/destination folder for schema and data files.")]
    public string? Folder { get; set; }

    [Option('l', "log-file", Required = false, HelpText = "Full path for log file.")]
    public string? LogFile { get; set; }

    [Option('q', "add-quotes", Required = false, HelpText = "Flag to quote all identifiers. Only applicable for Postgres, where there is a significant difference in behaviour when quoting identifiers. NOTE: Postgres reserved words are always quoted. For SQL Server, this flag is ignored, and identifiers will always be quoted.")]
    public bool AddQuotes { get; set; }

    [Option('s', "search-filter", Required = false, HelpText = "A string to filter table names or file names. Note that the syntax of the SearchFilter is different depending on the context. For copy in from a file system, use a RegEx in .NET format. E.g. \"\\b(clients|scopes)\\b\" will match \"clients.schema\" and \"scopes.schema\", but not \"someclients.schema\" nor \"clients2.schema\". For copy out from SQL Server, the SearchFilter is the rhs of a LIKE clause. E.g. \"a[sa][ya][sg]%\" to get all tables that starts with 'a' followed by \"sys\" or \"aag\" (but also \"asas\", \"aayg\", and other combinations). If you don't use a search-filter, all tables are copied.")]
    public string? SearchFilter { get; set; }

    [Option("empty-string", Required = false, HelpText = "Handle strings that contains whitespace only. Legal values are \"Single\" (an empty string is converted to single space), \"Empty\" (a single space is converted to empty string), \"ForceSingle\" (empty strings and whitespace is converted to a single space), \"ForceEmpty\" (all whitespace is removed). NOTE: This flag has no effect during export, or if strings contains any non-whitespace characters.")]
    public EmptyStringFlag? EmptyString { get; set; }

    public Dictionary<string, string?> ToAppSettings()
    {
        var appSettings = new Dictionary<string, string?>();
        if (!string.IsNullOrWhiteSpace(ConnectionString)) 
            appSettings.Add(Constants.Config.ConnectionString, ConnectionString);
        if (!string.IsNullOrWhiteSpace(ConnectionString)) 
            appSettings.Add(Constants.Config.Folder, Folder);
        if (!string.IsNullOrWhiteSpace(ConnectionString)) 
            appSettings.Add(Constants.Config.LogFile, LogFile);
        if (!string.IsNullOrWhiteSpace(ConnectionString)) 
            appSettings.Add(Constants.Config.AddQuotes, AddQuotes.ToString());
        if (!string.IsNullOrWhiteSpace(ConnectionString)) 
            appSettings.Add(Constants.Config.SearchFilter, SearchFilter);
        if (!string.IsNullOrWhiteSpace(ConnectionString)) 
            appSettings.Add(Constants.Config.EmptyString, EmptyString.ToString());

        return appSettings;
    }
}