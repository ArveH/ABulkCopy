using System.Data;

namespace Testing.Shared.SqlServer;

public class MssDbHelper
{
    private static readonly Lazy<MssDbHelper> LazyInstance =
        new(() => new MssDbHelper());

    private readonly string? _connectionString;

    private MssDbHelper()
    {
        var configuration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");
        _connectionString = configuration.GetConnectionString(TestConstants.Config.DbKey);
    }

    public static MssDbHelper Instance => LazyInstance.Value;
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
        var sqlString = $"if exists (select name from sys.objects where type='U' and name='{tableName}') drop table [{tableName}];";
        await ExecuteNonQuery(sqlString);
    }

    public async Task InsertIntoSingleColumnTable(
        string tableName,
        object? value,
        SqlDbType? dbType=null)
    {
        await using var sqlConnection = new SqlConnection(ConnectionString);
        await sqlConnection.OpenAsync();
        var sqlString = $"insert into [{tableName}] values (@Value);";
        await using var sqlCommand = new SqlCommand(sqlString, sqlConnection);
        if (dbType == null)
        {
            sqlCommand.Parameters.AddWithValue("@Value", value);
        }
        else
        {
            // Note: Since dbType is nullable, we have to cast to non-nullable
            // otherwise we get the wrong SqlParameter constructor
            // and dbType is treated as the value instead of the SqlDbType
            var sqlParameter = new SqlParameter("@Value", (SqlDbType)dbType);
            sqlParameter.Value = value;
            sqlCommand.Parameters.Add(sqlParameter);
        }
        await sqlCommand.ExecuteNonQueryAsync();
    }

    public async Task ExecuteNonQuery(string sqlString)
    {
        await using var sqlConnection = new SqlConnection(ConnectionString);
        await sqlConnection.OpenAsync();
        await using var sqlCommand = new SqlCommand(sqlString, sqlConnection);
        await sqlCommand.ExecuteNonQueryAsync();
    }
}