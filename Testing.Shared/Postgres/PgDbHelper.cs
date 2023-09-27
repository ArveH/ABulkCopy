using Microsoft.Extensions.Logging.Abstractions;

namespace Testing.Shared.Postgres;

public class PgDbHelper
{
    private static readonly Lazy<PgDbHelper> LazyInstance =
        new(() => new PgDbHelper());

    private readonly PgContext _pgContext;
    private static TableDefinition _tableDefinition = new(Rdbms.Pg)
    {
        Header = new()
        {
            Name = GlobalTestTableName,
            Schema = "public"
        },
        Columns = new List<IColumn>
        {
            new PostgresUuid(1, "TestId", false),
            new PostgresBigInt(2, "BigIntCol", true),
            new PostgresInt(3, "IntCol", true),
            new PostgresSmallInt(4, "SmallIntCol", true),
            new PostgresBoolean(5, "BooleanCol", true),
            new PostgresDate(6, "DateCol", true),
            new PostgresTimestamp(7, "TimestampCol", true),
            new PostgresTimestampTz(8, "TimestampTzCol", true),
            new PostgresTime(9, "TimeCol", true),
            new PostgresDoublePrecision(11, "DoublePrecisionCol", true),
            new PostgresReal(12, "RealCol", true),
            new PostgresMoney(13, "MoneyCol", true),
            new PostgresDecimal(14, "DecimalCol", true, 32, 6),
            new PostgresVarChar(15, "VarCharCol", true, 100),
            new PostgresChar(16, "CharCol", true, 10),
            new PostgresText(17, "TextCol", true),
            new PostgresUuid(18, "UuidCol", true)
        }
    };

    private PgDbHelper()
    {
        IConfiguration configuration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");
        _pgContext = new PgContext(new NullLoggerFactory(), configuration);
    }

    public static PgDbHelper Instance => LazyInstance.Value;
    public string ConnectionString => _pgContext.ConnectionString;
    public const string GlobalTestTableName = "PgIntegrationTestTable";

    public async Task VerifyTestTable()
    {
        await CreateTable(_tableDefinition, true);
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

            sb.Append($"    {identifier.Get(column.Name)} {column.GetNativeCreateClause()}");
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
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) throw new SqlNullValueException();

        if (reader.IsDBNull(0)) return 0L;

        return reader.GetFieldValue<long>(0);
    }

    public async Task<T?> SelectScalar<T>(string tableName, IColumn col, bool addQuote = true)
    {
        var identifier = GetIdentifier(addQuote);
        var sqlString = $"select {identifier.Get(col.Name)} from {identifier.Get(tableName)};";
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) throw new SqlNullValueException();

        if (reader.IsDBNull(0)) return default(T);

        return reader.GetFieldValue<T>(0);
    }

    public async Task<List<T?>> SelectColumn<T>(string tableName, string colName, bool addQuote = true)
    {
        var identifier = GetIdentifier(addQuote);
        var sqlString = $"select {identifier.Get(colName)} from {identifier.Get(tableName)};";
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
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
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        await cmd.ExecuteNonQueryAsync();
    }

    protected Identifier GetIdentifier(bool addQuote = true)
    {
        var appSettings = new Dictionary<string, string?>
        {
            {Constants.Config.AddQuotes, addQuote.ToString()}
        };
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettings)
            .Build();
        var identifier = new Identifier(config, _pgContext);
        return identifier;
    }
}