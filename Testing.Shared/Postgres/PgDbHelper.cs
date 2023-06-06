using Microsoft.Extensions.Logging.Abstractions;

namespace Testing.Shared.Postgres;

public class PgDbHelper
{
    private static readonly Lazy<PgDbHelper> LazyInstance =
        new(() => new PgDbHelper());

    private readonly PgContext _pgContext;

    private PgDbHelper()
    {
        var configuration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");
        var connectionString = configuration.GetConnectionString(TestConstants.Config.DbKey);
        if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
        _pgContext = new PgContext(new NullLoggerFactory()) { ConnectionString = connectionString };
    }

    public static PgDbHelper Instance => LazyInstance.Value;
    public string ConnectionString => _pgContext.ConnectionString;

    public async Task CreateTable(TableDefinition tableDefinition)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"create table \"{tableDefinition.Header.Name}\" (");
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

    public async Task<T> SelectScalar<T>(string tableName, string colName)
    {
        var sqlString = $"select \"{colName}\" from \"{tableName}\";";
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) throw new SqlNullValueException();
        
        // NOTE: The simple
        //     return (T)reader[0];
        // doesn't work for all types, e.g. cast Int16 to int32.
        return (T)Convert.ChangeType(reader[0], typeof(T));
    }

    public async Task ExecuteNonQuery(string sqlString)
    {
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        await cmd.ExecuteNonQueryAsync();
    }
}