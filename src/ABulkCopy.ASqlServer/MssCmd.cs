namespace ABulkCopy.ASqlServer;

public class MssCmd : MssCommandBase, IMssCmd
{
    private readonly IQueryBuilderFactory _queryBuilderFactory;

    public MssCmd(
        IDbContext dbContext,
        IQueryBuilderFactory queryBuilderFactory) : base(dbContext)
    {
        _queryBuilderFactory = queryBuilderFactory;
    }

    public async Task DropTableAsync(SchemaTableTuple st, CancellationToken ct)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await ExecuteNonQueryAsync(
            qb.DropTableStmt(st),
            ct).ConfigureAwait(false);
    }

    public async Task CreateTableAsync(
        TableDefinition tableDefinition,
        CancellationToken ct,
        bool addIfNotExists = false)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await ExecuteNonQueryAsync(
            qb.CreateTableStmt(tableDefinition, addIfNotExists),
            ct).ConfigureAwait(false);
    }

    public Task CreateIndexAsync(SchemaTableTuple st, IndexDefinition indexDefinition, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}