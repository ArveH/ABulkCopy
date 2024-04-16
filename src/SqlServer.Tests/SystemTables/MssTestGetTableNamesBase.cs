namespace SqlServer.Tests.SystemTables;

public class MssTestGetTableNamesBase(
    DatabaseFixture dbFixture, ITestOutputHelper output)
    : MssTestBase(dbFixture, output)
{
    protected async Task CreateTable(string tableName)
    {
        await CreateTable("dbo", tableName);
    }

    protected async Task CreateTable(string schema, string tableName)
    {
        await DbFixture.DropTable(schema, tableName);
        await DbFixture.ExecuteNonQuery(
            $"CREATE TABLE [{schema}].[{tableName}](\r\n\t[ExactNumBigInt] [bigint] NOT NULL)");
    }
}