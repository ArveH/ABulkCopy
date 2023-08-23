namespace ABulkCopy.Cmd;

public class CmdArguments
{
    [Option('d', "Direction", Required = true, HelpText = "Copy direction. \"In\" to a database or \"Out\" from a database.")]
    public required CopyDirection Direction { get; set; }

    [Option('c', "Connection", Required = true, HelpText = "The connection string")]
    public required string ConnectionString { get; set; }

    [Option("EmptyString", Required = false, HelpText = "Trim strings containing whitespace only. Legal values are \"Single\" (an empty string is converted to single space), \"Empty\" (a single space is converted to empty string), \"ForceSingle\" (empty strings and all whitespace is converted to a single space), \"ForceEmpty\" (all whitespace is removed). NOTE: This flag has no effect during export, or if strings contains any non-whitespace characters.")]
    public EmptyStringFlag EmptyString { get; set; } = EmptyStringFlag.Leave;

    [Option('f', "Folder", Required = false, Default = "\\.", HelpText = "The source/destination folder for schema and data files.")]
    public string Folder { get; set; } = "\\.";

    [Option('l', "Log", Required = false, Default = "\\.\\abulkcopy.log", HelpText = "Full path for log file.")]
    public string LogFile { get; set; } = "\\.\\abulkcopy.log";

    [Option('r', "Rdbms", Required = true, HelpText = "Database Management System. \"Pg\" or \"Mss\".")]
    public required Rdbms Rdbms { get; set; }

    [Option('s', "SearchFilter", Required = false, Default = "", HelpText = "A string to filter table names or file names. Note that the syntax of the SearchFilter is different depending on the context. For copy in from a file system, use a RegEx in .NET format. E.g. \"\\b(clients|scopes)\\b\" will match \"clients.schema\" and \"scopes.schema\", but not \"someclients.schema\" nor \"clients2.schema\". For copy out from SQL Server, the SearchFilter is the rhs of a LIKE clause. E.g. \"a[sa][ya][sg]%\" to get all tables that starts with 'a' followed by \"sys\" or \"aag\" (or \"sas\" \"ayg\", etc). If you don't use SearchFilter, all tables are copied.")]
    public string SearchStr { get; set; } = "";

    [Option('b', "batch", Required = false, Default = 1000, HelpText = "Batch size. Currently not used")]
    public int BatchSize { get; set; } = 1000;
}