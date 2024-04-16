namespace SqlServer.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class MssTestGetTableNamesMultipleSchemas(
    DatabaseFixture dbFixture, ITestOutputHelper output)
    : MssTestGetTableNamesBase(dbFixture, output)
{
    [Fact]
    public async Task TestGetTableNames_When_SameTableInDifferentSchemas()
    {
        var guid = Guid.NewGuid().ToString("N");
        try
        {
            // Arrange
            var testTableName = "T_" + guid + "1";
            await CreateTableAsync(DatabaseFixture.TestSchemaName, testTableName);
            await CreateTableAsync("dbo", testTableName);
            await CreateTableAsync("dbo", "T_ExtraTable");

            // Act
            var tableNames = await MssSystemTables.GetTableNamesAsync(testTableName, CancellationToken.None);

            // Assert
            var tableList = tableNames.ToList();
            tableList.Count().Should().Be(2);
            tableList[0].Should().Be("dbo." + testTableName);
            tableList[1].Should().Be($"{DatabaseFixture.TestSchemaName}." + testTableName);
        }
        finally
        {
            await DbFixture.DropTable(DatabaseFixture.TestSchemaName, "T_" + guid + "1");
            await DbFixture.DropTable("dbo", "T_" + guid + "1");
            await DbFixture.DropTable("dbo", "T_" + guid + "2");
            await DbFixture.DropTable("dbo", "T_" + guid + "3");
            await DbFixture.DropTable("dbo", "T_MyNewData");
        }
    }
}