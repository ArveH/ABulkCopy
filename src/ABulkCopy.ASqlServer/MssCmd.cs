namespace ABulkCopy.ASqlServer;

public class MssCmd : IMssCmd
{
    private readonly IMssRawCommand _rawCommand;
    private readonly IQueryBuilderFactory _queryBuilderFactory;
    private readonly ILogger _logger;

    public MssCmd(
        IMssRawCommand rawCommand,
        IQueryBuilderFactory queryBuilderFactory,
        ILogger logger)
    {
        _rawCommand = rawCommand;
        _queryBuilderFactory = queryBuilderFactory;
        _logger = logger;
    }

    public async Task DropTableAsync(
        SchemaTableTuple st, 
        CancellationToken ct)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await _rawCommand.ExecuteNonQueryAsync(
            qb.DropTableStmt(st),
            ct).ConfigureAwait(false);
    }

    public async Task CreateTableAsync(
        TableDefinition tableDefinition,
        CancellationToken ct,
        bool addIfNotExists = false)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await _rawCommand.ExecuteNonQueryAsync(
            qb.CreateTableStmt(tableDefinition, addIfNotExists),
            ct).ConfigureAwait(false);
    }

    public async Task CreateIndexAsync(
        SchemaTableTuple st, 
        IndexDefinition indexDefinition,
        CancellationToken ct)
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
        sb.Append($"[{st.schemaName}].[{st.tableName}] (");
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

        await _rawCommand.ExecuteNonQueryAsync(
            sb.ToString(), ct).ConfigureAwait(false);
    }

    public async Task DropIndexAsync(
        SchemaTableTuple st, 
        string indexName,
        CancellationToken ct)
    {
        var sqlString = $"if exists (select name from sys.indexes where object_id=object_id('{st.tableName}') " + 
                        $"and name = '{indexName}' drop index [{st.tableName}].[{indexName}];";
        await _rawCommand.ExecuteNonQueryAsync(
            sqlString, CancellationToken.None).ConfigureAwait(false);
    }
    
    public async Task EnsureSchemaAsync(string name)
    {
        var schemaExists = (int?)await _rawCommand.ExecuteScalarAsync(
            $"select count(*) from sys.schemas where name = '{name}'",
            CancellationToken.None).ConfigureAwait(false);
        if (schemaExists is 0)
        {
            await _rawCommand.ExecuteNonQueryAsync(
                $"CREATE SCHEMA {name}", CancellationToken.None).ConfigureAwait(false);
        }
    }
}