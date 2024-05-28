using Serilog;

namespace Testing.Shared.SqlServer;

public class MssDbHelper : MssCommandBase
{
    private readonly IDbContext _dbContext;
    private readonly IQueryBuilderFactory _queryBuilderFactory;
    private readonly ILogger _logger;
    public const string TestSchemaName = "my_mss_schema";

    public MssDbHelper(
        IDbContext dbContext,
        IQueryBuilderFactory queryBuilderFactory,
        ILogger logger) : base(dbContext)
    {
        _dbContext = dbContext;
        _queryBuilderFactory = queryBuilderFactory;
        _logger = logger;
    }

    public async Task EnsureTestSchemaAsync()
    {
        var schemaExists = await ExecuteScalarAsync<int>($"select count(*) from sys.schemas where name = '{TestSchemaName}'");
        if (schemaExists == 0)
        {
            await ExecuteNonQueryAsync($"CREATE SCHEMA {TestSchemaName}", CancellationToken.None);
        }
    }

    public async Task CreateTableAsync(TableDefinition tableDefinition)
    {
        var stmt = _queryBuilderFactory.GetQueryBuilder().CreateTableStmt(tableDefinition);
        _logger.Verbose("Create table statement: {SqlStmt}", stmt);
        await ExecuteNonQueryAsync(stmt, CancellationToken.None);
    }

    public async Task DropTableAsync(SchemaTableTuple st)
    {
        var stmt = _queryBuilderFactory.GetQueryBuilder().DropTableStmt(st);
        _logger.Verbose("Drop table statement: {SqlStmt}", stmt);
        await ExecuteNonQueryAsync(stmt, CancellationToken.None);
    }

    public async Task InsertIntoSingleColumnTableAsync(
        string tableName,
        object? value,
        SqlDbType? dbType = null)
    {
        await using var sqlConnection = new SqlConnection(_dbContext.ConnectionString);
        await sqlConnection.OpenAsync();
        var sqlString = $"insert into [{tableName}] values (@Value);";
        await using var sqlCommand = new SqlCommand(sqlString, sqlConnection);
        if (dbType == null)
        {
            sqlCommand.Parameters.AddWithValue("@Value", value ?? DBNull.Value);
        }
        else
        {
            // Note: Since dbType is nullable, we have to cast to non-nullable
            // otherwise we get the wrong SqlParameter constructor
            // and dbType is treated as the value instead of the SqlDbType
            var sqlParameter = new SqlParameter("@Value", (SqlDbType)dbType);
            sqlParameter.Value = value ?? DBNull.Value;
            sqlCommand.Parameters.Add(sqlParameter);
        }
        await sqlCommand.ExecuteNonQueryAsync();
    }

    public async Task CreateIndexAsync(string tableName, IndexDefinition indexDefinition)
    {
        var sb = new StringBuilder();
        sb.Append("create ");
        if (indexDefinition.Header.IsUnique)
        {
            sb.Append("unique ");
        }

        if (indexDefinition.Header.Type == IndexType.Clustered)
        {
            sb.Append("clustered ");
        }

        sb.Append("index ");
        sb.Append($"[{indexDefinition.Header.Name}] on ");
        sb.Append($"[{tableName}] (");
        var first = true;
        foreach (var indexCol in indexDefinition.Columns)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                sb.AppendLine(",");
            }

            sb.Append($"    {indexCol.Name} ");
            if (indexCol.Direction == Direction.Descending)
            {
                sb.Append("desc");
            }
        }
        sb.Append(")");

        await ExecuteNonQueryAsync(sb.ToString(), CancellationToken.None);
    }

    public async Task DropIndexAsync(string tableName, string indexName)
    {
        var sqlString = $"if exists (select name from sys.indexes where object_id=object_id('{tableName}') and name = '{indexName}' drop index [{tableName}].[{indexName}];";
        await ExecuteNonQueryAsync(sqlString, CancellationToken.None);
    }

    public async Task<T?> ExecuteScalarAsync<T>(string sqlString)
    {
        await using var sqlConnection = new SqlConnection(_dbContext.ConnectionString);
        await sqlConnection.OpenAsync();
        await using var sqlCommand = new SqlCommand(sqlString, sqlConnection);
        return (T?)await sqlCommand.ExecuteScalarAsync();
    }

}