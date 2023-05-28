namespace ASqlServer.Test;

public class MssGetIndexesTest : MssTestBase
{
    private readonly string _testTableName = Environment.MachineName + "MssGetIndexesTest";
    private readonly string _testIndexName = Environment.MachineName + "IX_MssGetIndexesTest";

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
        await MssDbHelper.Instance.DropTable(_testTableName);
        var tableHeader = await CreateTestTable();
        tableHeader.Should().NotBeNull("because table should exist");
        var columns = new List<IndexColumn>
        {
            new() { Name = "Col1" },
            new() { Name = "Col2", Direction = Direction.Descending},
            new() { Name = "Col3" }
        };
        await CreateIndex(tableHeader!.Id, columns);

        // Act
        var indexes = (await MssSystemTables.GetIndexes(tableHeader)).ToList();
        indexes.Should().NotBeNull();
        indexes.Count.Should().Be(1);
        indexes[0].Columns = (await MssSystemTables.GetIndexColumnInfo(_testTableName, indexes[0].Header)).ToList();

        // Assert
        indexes[0].Header.Id.Should().BeGreaterThan(0, "because index_id = 0 is of type Heap");
        indexes[0].Header.TableId.Should().Be(tableHeader.Id);
        indexes[0].Header.Name.Should().Be(_testIndexName);
        indexes[0].Columns.Count.Should().Be(3);
        indexes[0].Columns[0].Name.Should().Be("Col1");
        indexes[0].Columns[0].Direction.Should().Be(Direction.Ascending);
        indexes[0].Columns[1].Name.Should().Be("Col2");
        indexes[0].Columns[1].Direction.Should().Be(Direction.Descending);
        indexes[0].Columns[2].Name.Should().Be("Col3");
        indexes[0].Columns[2].Direction.Should().Be(Direction.Ascending);
    }

    private async Task<TableHeader?> CreateTestTable()
    {
        var tableDef = MssTestData.GetEmpty(_testTableName);
        tableDef.Columns.Add(new SqlServerInt(1, "Col1", false));
        tableDef.Columns.Add(new SqlServerInt(2, "Col2", false));
        tableDef.Columns.Add(new SqlServerInt(3, "Col3", false));
        await MssDbHelper.Instance.CreateTable(tableDef);
        return await MssSystemTables.GetTableHeader(_testTableName);
    }

    private async Task CreateIndex(int tableId, List<IndexColumn> columns)
    {
        var indexDef = new IndexDefinition
        {
            Header = new IndexHeader
            {
                TableId = tableId,
                Name = _testIndexName,
                IsUnique = false,
                Type = IndexType.NonClustered,
                Location = "PRIMARY"
            },
            Columns = columns
        };

        await MssDbHelper.Instance.CreateIndex(_testTableName, indexDef);
    }
}