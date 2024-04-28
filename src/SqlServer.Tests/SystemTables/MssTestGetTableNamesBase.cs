namespace SqlServer.Tests.SystemTables;

public class MssTestGetTableNamesBase(
    DatabaseFixture dbFixture, ITestOutputHelper output)
    : MssTestBase(dbFixture, output)
{
    protected async Task CreateTableAsync(string tableName)
    {
        await CreateTableAsync("dbo", tableName);
    }

    protected async Task CreateTableAsync(string schema, string tableName)
    {
        await DbFixture.DropTable(schema, tableName);
        await DbFixture.ExecuteNonQuery(
            $"CREATE TABLE [{schema}].[{tableName}](\r\n\t[ExactNumBigInt] [bigint] NOT NULL)");
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
                await DbFixture.DropTable(table.schemaName, table.tableName);
            }
        }
    }
}