namespace SqlServer.Tests.SystemTables;

public class MssTestGetTableNamesBase(
    DatabaseFixture dbFixture, ITestOutputHelper output)
    : MssTestBase(dbFixture, output)
{
    protected async Task TestGetTableNamesAsync(string guid, string searchString, int expectedCount)
    {
        try
        {
            // Arrange
            await CreateTable_For_TestGetTableNames("T_" + guid + "1");
            await CreateTable_For_TestGetTableNames("T_" + guid + "2");
            await CreateTable_For_TestGetTableNames("T_" + guid + "3");
            await CreateTable_For_TestGetTableNames("T_MyNewData");

            // Act
            var tableNames = await MssSystemTables.GetTableNamesAsync(searchString, CancellationToken.None);

            // Assert
            tableNames.Count().Should().Be(expectedCount);
        }
        finally
        {
            await DbFixture.DropTable("T_" + guid + "1");
            await DbFixture.DropTable("T_" + guid + "2");
            await DbFixture.DropTable("T_" + guid + "3");
            await DbFixture.DropTable("T_MyNewData");
        }
    }

    protected async Task CreateTable_For_TestGetTableNames(string tableName)
    {
        await DbFixture.DropTable(tableName);
        await DbFixture.ExecuteNonQuery(
            $"CREATE TABLE [dbo].[{tableName}](\r\n\t[ExactNumBigInt] [bigint] NOT NULL)");
    }
}