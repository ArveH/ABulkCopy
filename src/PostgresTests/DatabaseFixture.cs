using Microsoft.Extensions.Logging.Abstractions;

namespace PostgresTests;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _pgContainer =
        new PostgreSqlBuilder()
            .WithPortBinding(54771, 5432)
            .Build();
    private IPgContext? _pgContext;
    private readonly IParseTree _parseTree = new ParseTree(new NodeFactory(), new SqlTypes());
    private readonly IPgParser _parser = new PgParser();
    private readonly ITokenizerFactory _tokenizerFactory = new TokenizerFactory(new TokenFactory());
    private IConfiguration? _configuration;

    public string PgConnectionString => _pgContainer.GetConnectionString();
    public string PgContainerId => _pgContainer.Id;

    public IConfiguration Configuration 
        => _configuration ?? throw new ArgumentNullException(nameof(Configuration));

    public IPgContext PgContext
    {
        get => _pgContext ?? throw new ArgumentNullException(nameof(PgContext));
        private set => _pgContext = value;
    }

    public async Task InitializeAsync()
    {
        await _pgContainer.StartAsync();
        _configuration = new ConfigHelper().GetConfiguration(
            "ed7ee99b-0e84-4e9a-9eb5-985d610aeb8b",
            new()
            {
                { Constants.Config.ConnectionString, PgConnectionString },
                { Constants.Config.AddQuotes, "true" }
        });
        PgContext = new PgContext(new NullLoggerFactory(), Configuration);
    }

    public async Task CreateTable(TableDefinition tableDefinition,
    bool addQuote = true,
    bool addIfNotExists = false)
    {
        var sb = new StringBuilder();
        var identifier = GetIdentifier(addQuote);
        sb.Append("create table ");
        if (addIfNotExists) sb.Append("if not exists ");
        sb.Append(identifier.Get(tableDefinition.Header.Name));
        sb.AppendLine("(");
        var first = true;
        foreach (var column in tableDefinition.Columns)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                sb.AppendLine(",");
            }

            sb.Append($"    {identifier.Get(column.Name)} ");
            sb.Append(column.GetTypeClause());
            sb.Append(column.GetIdentityClause());
            if (column.HasDefault)
            {
                var tokenizer = _tokenizerFactory.GetTokenizer();
                tokenizer.Initialize(column.DefaultConstraint!.Definition);
                tokenizer.GetNext();
                var root = _parseTree.CreateExpression(tokenizer);
                sb.Append(" default ");
                sb.Append(_parser.Parse(tokenizer, root));
            }

            sb.Append(column.GetNullableClause());
        }

        sb.AppendLine(");");
        await ExecuteNonQuery(sb.ToString());
    }

    public async Task DropTable(string tableName, bool addQuote = true)
    {
        var identifier = GetIdentifier(addQuote);
        var sqlString = $"drop table if exists {identifier.Get(tableName)};";
        await ExecuteNonQuery(sqlString);
    }

    public async Task<long> GetRowCount(string tableName, bool addQuote = true)
    {
        var identifier = GetIdentifier(addQuote);
        var sqlString = $"select count(*) from {identifier.Get(tableName)};";
        await using var cmd = PgContext.DataSource.CreateCommand(sqlString);
        var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) throw new SqlNullValueException();

        if (reader.IsDBNull(0)) return 0L;

        return reader.GetFieldValue<long>(0);
    }

    public async Task<T?> SelectScalar<T>(string tableName, IColumn col, bool addQuote = true)
    {
        var identifier = GetIdentifier(addQuote);
        var sqlString = $"select {identifier.Get(col.Name)} from {identifier.Get(tableName)};";
        await using var cmd = PgContext.DataSource.CreateCommand(sqlString);
        var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) throw new SqlNullValueException();

        if (reader.IsDBNull(0)) return default(T);

        return reader.GetFieldValue<T>(0);
    }

    public async Task<List<T?>> SelectColumn<T>(string tableName, string colName, bool addQuote = true)
    {
        var identifier = GetIdentifier(addQuote);
        var sqlString = $"select {identifier.Get(colName)} from {identifier.Get(tableName)};";
        await using var cmd = PgContext.DataSource.CreateCommand(sqlString);
        var reader = await cmd.ExecuteReaderAsync();
        var result = new List<T?>();
        while (await reader.ReadAsync())
        {
            result.Add(reader.IsDBNull(0) ? default : reader.GetFieldValue<T>(0));
        }

        return result;
    }

    public async Task ExecuteNonQuery(string sqlString)
    {
        await using var cmd = PgContext.DataSource.CreateCommand(sqlString);
        await cmd.ExecuteNonQueryAsync();
    }

    protected Identifier GetIdentifier(bool addQuote = true)
    {
        var appSettings = new Dictionary<string, string?>
        {
            { Constants.Config.AddQuotes, addQuote.ToString() }
        };
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettings)
            .Build();
        var identifier = new Identifier(config, PgContext);
        return identifier;
    }

    public async Task DisposeAsync()
    {
        await _pgContainer.DisposeAsync();
        _pgContext?.Dispose();
    }
}