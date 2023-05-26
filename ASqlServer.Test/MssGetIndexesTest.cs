namespace ASqlServer.Test;

public class MssGetIndexesTest : MssTestBase
{
    private readonly string _testTableName = Environment.MachineName + "MssGetIndexesTest";

    public MssGetIndexesTest(ITestOutputHelper output) 
        : base(output)
    {
    }

    [Fact]
    public async Task TestGetIndexes_When_OneIndexOneColumn()
    {
        // Arrange
        var tableHeader = await MssSystemTables.GetTableHeader("ConfiguredClients");
        tableHeader.Should().NotBeNull();

        // Act
        var indexes = (await MssSystemTables.GetIndexes(tableHeader!)).ToList();

        // Assert
        indexes.Should().NotBeNull();
        indexes.Count.Should().Be(1);
        indexes[0].Header.Id.Should().Be(2);
        indexes[0].Header.TableId.Should().BeGreaterThan(0);
        indexes[0].Header.Name.Should().Be("IX_ConfiguredClients_OwnerTenant");
        indexes[0].Header.IsUnique.Should().BeFalse();
        indexes[0].Header.Type.Should().Be(IndexType.NonClustered);
        indexes[0].Header.Location.Should().Be("PRIMARY");
    }

    [Fact]
    public async Task TestGetIndexes_When_OneDescendingColumn()
    {
        // Arrange
        await CreateTestTable();
        var tableDef = MssTestData.GetEmpty(_testTableName);
        tableDef.Columns.Add(new SqlServerInt(1, "Col1", false));
        tableDef.Columns.Add(new SqlServerInt(2, "Col2", false));
        tableDef.Columns.Add(new SqlServerInt(3, "Col3", false));


    }

    private async Task CreateTestTable()
    {
        var tableDef = MssTestData.GetEmpty(_testTableName);
        tableDef.Columns.Add(new SqlServerInt(1, "Col1", false));
        tableDef.Columns.Add(new SqlServerInt(2, "Col2", false));
        tableDef.Columns.Add(new SqlServerInt(3, "Col3", false));
        await MssDbHelper.Instance.CreateTable(tableDef);
        var tableHeader = await MssSystemTables.GetTableHeader("ConfiguredClients");

        var indexDef = new IndexDefinition
        {
            Header = new IndexHeader
            {
                TableId = tableDef.Header.Id,
                Name = "IX_" + _testTableName + "_Col1",
                IsUnique = false,
                Type = IndexType.NonClustered,
                Location = "PRIMARY"
            },
        };
    }
}