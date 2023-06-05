namespace Testing.Shared.Postgres;

public class PgDbHelper
{
    private static readonly Lazy<PgDbHelper> LazyInstance = 
        new(() => new PgDbHelper());

    private readonly string? _connectionString;

    private PgDbHelper()
    {
        var configuration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");
        _connectionString = configuration.GetConnectionString(TestConstants.Config.DbKey);
    }

    public static PgDbHelper Instance => LazyInstance.Value;
    public string ConnectionString => _connectionString ?? throw new ArgumentNullException(nameof(ConnectionString));

    public async Task CreateTable(TableDefinition tableDefinition)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"create table [{tableDefinition.Header.Name}] (");
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

            sb.Append($"    {column.Name} {column.GetNativeCreateClause()}");
        }
        sb.AppendLine(");");
        await ExecuteNonQuery(sb.ToString());
    }

    public async Task DropTable(string tableName)
    {
        var sqlString = $"drop table if exists [{tableName}];";
        await ExecuteNonQuery(sqlString);
    }

    public async Task ExecuteNonQuery(string sqlString)
    {
        await using var sqlConnection = new SqlConnection(ConnectionString);
        await sqlConnection.OpenAsync();
        await using var sqlCommand = new SqlCommand(sqlString, sqlConnection);
        await sqlCommand.ExecuteNonQueryAsync();
    }
}