namespace Postgres.Tests.SystemTables;

public class PgTestGetTableNamesBase(
    DatabaseFixture dbFixture, ITestOutputHelper output)
    : PgTestBase(dbFixture, output)
{
    private async Task CreateTableAsync(string schema, string tableName)
    {
        await DropTableAsync(schema, tableName);
        await ExecuteNonQueryAsync(
            $"CREATE TABLE {schema}.{tableName}(\r\n\tExactNumBigInt bigint NOT NULL)");
    }

    protected async Task<List<SchemaTableTuple>> CreateAndCleanupTablesAsync(
        List<SchemaTableTuple> tabTuples,
        Func<Task<List<SchemaTableTuple>>> act)
    {
        try
        {
            foreach (var tabTuple in tabTuples)
            {
                await CreateTableAsync(tabTuple.schemaName, tabTuple.tableName);
            }

            return await act();
        }
        finally
        {
            foreach (var table in tabTuples)
            {
                await DropTableAsync(table.schemaName, table.tableName);
            }
        }
    }
}