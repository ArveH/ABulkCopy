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
        var configuration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");
        var connectionString = configuration.GetConnectionString(TestConstants.Config.DbKey);
        if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
        _pgContext = new PgContext(new NullLoggerFactory()) { ConnectionString = connectionString };
    }

    public static PgDbHelper Instance => LazyInstance.Value;
    public string ConnectionString => _pgContext.ConnectionString;
    public const string GlobalTestTableName = "PgIntegrationTestTable";

    public async Task VerifyTestTable()
    {
        await CreateTable(_tableDefinition, true);
    }

    public async Task CreateTable(TableDefinition tableDefinition, bool addIfNotExists = false)
    {
        var sb = new StringBuilder();
        sb.Append("create table ");
        if (addIfNotExists) sb.Append("if not exists ");
        sb.AppendLine($"\"{tableDefinition.Header.Name}\" (");
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

            sb.Append($"    \"{column.Name}\" {column.GetNativeCreateClause()}");
        }
        sb.AppendLine(");");
        await ExecuteNonQuery(sb.ToString());
    }

    public async Task DropTable(string tableName)
    {
        var sqlString = $"drop table if exists \"{tableName}\";";
        await ExecuteNonQuery(sqlString);
    }

    public async Task<T?> SelectScalar<T>(string tableName, IColumn col)
    {
        var sqlString = $"select \"{col.Name}\" from \"{tableName}\";";
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) throw new SqlNullValueException();

        if (reader.IsDBNull(0)) return default(T);

        return reader.GetFieldValue<T>(0);
    }

    public async Task<List<T?>> SelectColumn<T>(string tableName, string colName)
    {
        var sqlString = $"select \"{colName}\" from \"{tableName}\";";
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
}