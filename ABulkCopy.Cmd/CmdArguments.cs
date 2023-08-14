namespace ABulkCopy.Cmd;

public class CmdArguments
{
    [Option('d', "direction", Required = true, HelpText = "Copy direction. \"In\" to a database or \"Out\" from a database.")]
    public required CopyDirection Direction { get; set; }

    [Option('c', "connection", Required = true, HelpText = "The connection string")]
    public required string ConnectionString { get; set; }

    [Option("empty_string", Required = false, HelpText = "Trim strings containing whitespace only. Legal values are \"Single\" (an empty string is converted to single space), \"Empty\" (a single space is converted to empty string), \"ForceSingle\" (empty strings and all whitespace is converted to a single space), \"ForceEmpty\" (all whitespace is removed). NOTE: This flag has no effect during export, or if strings contains any non-whitespace characters.")]
    public EmptyStringFlag EmptyString { get; set; } = EmptyStringFlag.Leave;

    [Option('f', "folder", Required = false, Default = "\\.", HelpText = "The source/destination folder for schema and data files.")]
    public string Folder { get; set; } = "\\.";

    [Option('l', "log", Required = false, Default = "\\.\\abulkcopy.log", HelpText = "Full path for log file.")]
    public string LogFile { get; set; } = "\\.\\abulkcopy.log";

    [Option('r', "rdbms", Required = true, HelpText = "Database Management System. \"Pg\" or \"Mss\".")]
    public required Rdbms Rdbms { get; set; }

    [Option('x', "regex", Required = false, Default = "", HelpText = "A RegEx to filter table names or file names. E.g. \"\\b(clients|scopes)\\b\" will match clients.schema and scopes.schema, but not someclients.schema nor clients2.schema")]
    public string SearchStr { get; set; } = "";

    [Option('b', "batch", Required = false, Default = 1000, HelpText = "Batch size. Currently not used")]
    public int BatchSize { get; set; } = 1000;
}