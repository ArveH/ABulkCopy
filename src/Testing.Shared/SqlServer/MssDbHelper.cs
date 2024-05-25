using ABulkCopy.ASqlServer;
using ABulkCopy.Common.Database;

namespace Testing.Shared.SqlServer;

public class MssDbHelper : MssCommandBase
{
    private readonly IDbContext _dbContext;

    public MssDbHelper(IDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateTable(TableDefinition tableDefinition)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"create table [{tableDefinition.Header.Schema}].[{tableDefinition.Header.Name}] (");
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

            sb.Append($"    {column.Name} ");
            sb.Append(column.GetTypeClause());
            sb.Append(column.GetIdentityClause());
            if (column.HasDefault)
            {
                throw new NotImplementedException("Test can't handle default values");
            }
            sb.Append(column.GetNullableClause());
        }
        sb.AppendLine(");");
        await ExecuteNonQueryAsync(sb.ToString(), CancellationToken.None);
    }

    public async Task DropTable(SchemaTableTuple st)
    {
        await ExecuteNonQueryAsync($"DROP TABLE IF EXISTS [{st.schemaName}].[{st.tableName}];", CancellationToken.None);
    }

    public async Task InsertIntoSingleColumnTable(
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

    public async Task CreateIndex(string tableName, IndexDefinition indexDefinition)
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

    public async Task DropIndex(string tableName, string indexName)
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