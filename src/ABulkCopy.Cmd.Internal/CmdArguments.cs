namespace ABulkCopy.Cmd.Internal;

public class CmdArguments
{
    [Option('d', "direction", Required = true, HelpText = "Copy direction. \"In\" to a database or \"Out\" from a database.")]
    public required CopyDirection Direction { get; set; }

    [Option('c', "connection-string", Required = true, HelpText = "The connection string")]
    public string? ConnectionString { get; set; }

    [Option('r', "rdbms", Required = true, HelpText = "Database Management System. \"Pg\" or \"Mss\".")]
    public required Rdbms Rdbms { get; set; }

    [Option('q', "add-quotes", Required = false, HelpText = "Flag to quote all identifiers. Only applicable for Postgres, where there is a significant difference in behaviour when quoting identifiers. NOTE: Postgres reserved words are always quoted. For SQL Server, this flag is ignored, and identifiers will always be quoted. This parameter is only used when direction = In")]
    public bool AddQuotes { get; set; }

    [Option("empty-string", Required = false, HelpText = "Handle strings that contains whitespace only. Legal values are \"Single\" (an empty string is converted to single space), \"Empty\" (a single space is converted to empty string), \"ForceSingle\" (empty strings and whitespace is converted to a single space), \"ForceEmpty\" (all whitespace is removed). NOTE: This flag has no effect during export, or if strings contains any non-whitespace characters.")]
    public EmptyStringFlag? EmptyString { get; set; }

    [Option('f', "folder", Required = false, HelpText = "The source/destination folder for schema and data files.")]
    public string? Folder { get; set; }

    [Option('l', "log-file", Required = false, HelpText = "Full path for log file.")]
    public string? LogFile { get; set; }

    [Option('m', "mappings-file", Required = false, HelpText = "The path and file name of a json file containing key-value pairs for mapping schema names, collation names and some simple type mapping. E.g. mapping the \"dbo\" schema in SQL Server to the \"public\" schema in Postgres. There is a sample-mappings.json file accompanying the executable. For column types, you should only map bit and datetime types (see documentation: https://arveh.github.io/ABulkCopy.Docs/command_line_parameters/#-m-mappings-file-in-only). This parameter is only used when direction = In")]
    public string? MappingsFile { get; set; }

    [Option("schema-filter", Required = false, HelpText = "A comma separated list of schema names. When it's not used, all schemas will be copied, except 'guest', 'information_schema', 'sys' and 'logs'. This parameter is only used when direction = Out")]
    public string? SchemaFilter { get; set; }

    [Option('s', "search-filter", Required = false, HelpText = "A string to filter table names or file names. Note that the syntax of the SearchFilter is different depending on the context. For copy in from a file system, use a RegEx in .NET format. E.g. \"\\b(clients|scopes)\\b\" will match \"clients.schema\" and \"scopes.schema\", but not \"someclients.schema\" nor \"clients2.schema\". For copy out from SQL Server, the SearchFilter is the rhs of a LIKE clause. E.g. \"a[sa][ya][sg]%\" to get all tables that starts with 'a' followed by \"sys\" or \"aag\" (but also \"asas\", \"aayg\", and other combinations). If you don't use a search-filter, all tables are copied.")]
    public string? SearchFilter { get; set; }

    [Option("skip-create", Required = false, HelpText = "This is an experimental parameter. It assumes that the tables already exists in the database, and will skip the \"create table\" step. The thought is that tables are created using Entity Framework migrations, then ABulkCopy is used to insert data. NOTE: Schema files are still needed to create the dependency graph and the copy statements, and they MUST correspond to the tables already in the database. This parameter is only used when direction = In")]
    public bool SkipCreate { get; set; }

    [Option("to-lower", Required = false, HelpText = "When importing tables, all identifiers (table names, column names, etc.) will be converted to lowercase. NOTE: For Postgres, this parameter will probably have no effect, unless it's used in conjunction with the --add-quotes parameter. Postgres will \"fold\" the identifier names to lowercase anyway, unless the identifier is surrounded with quotes. This parameter is only used when direction = In")]
    public bool ToLower { get; set; }


    public Dictionary<string, string?> ToAppSettings()
    {
        var appSettings = new Dictionary<string, string?>();
        if (!string.IsNullOrWhiteSpace(ConnectionString))
        {
            if (Rdbms == Rdbms.Mss)
            {
                appSettings.Add("ConnectionStrings:" + Constants.Config.MssConnectionString, ConnectionString);
            }
            else if (Rdbms == Rdbms.Pg)
            {
                appSettings.Add("ConnectionStrings:" + Constants.Config.PgConnectionString, ConnectionString);
            }
        }

        if (!string.IsNullOrWhiteSpace(Folder))
        {
            appSettings.Add(Constants.Config.Folder, Folder);
        }

        if (!string.IsNullOrWhiteSpace(MappingsFile))
        {
            appSettings.Add(Constants.Config.MappingsFile, MappingsFile);
        }
        if (!string.IsNullOrWhiteSpace(LogFile))
        {
            appSettings.Add(Constants.Config.LogFile, LogFile);
        }
        appSettings.Add(Constants.Config.AddQuotes, AddQuotes.ToString());
        appSettings.Add(Constants.Config.SkipCreate, SkipCreate.ToString());
        appSettings.Add(Constants.Config.ToLower, ToLower.ToString());
        if (!string.IsNullOrWhiteSpace(SchemaFilter))
        {
            appSettings.Add(Constants.Config.SchemaFilter, SchemaFilter);
        }
        if (!string.IsNullOrWhiteSpace(SearchFilter))
        {
            appSettings.Add(Constants.Config.SearchFilter, SearchFilter);
        }
        appSettings.Add(Constants.Config.EmptyString, EmptyString.ToString());

        return appSettings;
    }

    public static CmdArguments? Create(string[] args)
    {
        var parser = new Parser(cfg =>
        {
            cfg.CaseInsensitiveEnumValues = true;
            cfg.HelpWriter = Console.Error;
        });
        var result = parser.ParseArguments<CmdArguments>(args);
        if (result.Tag == ParserResultType.NotParsed)
        {
            // A usage message is written to Console.Error by the CommandLineParser
            return null;
        }

        return result.Value;
    }
}