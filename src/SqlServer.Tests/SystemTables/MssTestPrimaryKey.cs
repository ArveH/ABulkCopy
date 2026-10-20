namespace SqlServer.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class MssTestPrimaryKey : MssTestBase
{
    public MssTestPrimaryKey(DatabaseFixture dbFixture, ITestOutputHelper output) 
        : base(dbFixture, output)
    {
    }
    
    [Fact]
    public async Task TestGetPrimaryKey_When_Exists()
    {
        // Arrange
        var tableName = GetName();
        await DropTableAsync(tableName);
        await ExecuteNonQueryAsync(
            $"CREATE TABLE [{tableName}]([Key1] int NOT NULL, [Key2] int NOT NULL, [AnotherCol] nvarchar(20), " + Environment.NewLine +
            $"CONSTRAINT [PK_{tableName}] PRIMARY KEY CLUSTERED " + Environment.NewLine +
            "(" + Environment.NewLine +
            "\t[Key1] ASC," + Environment.NewLine +
            "\t[Key2] ASC" + Environment.NewLine +
            "))");
        var tableHeader = await MssSystemTables.GetTableHeaderAsync(
            "dbo", tableName, CancellationToken.None);
        tableHeader.Should().NotBeNull();

        // Act
        var pk = await MssSystemTables.GetPrimaryKeyAsync(tableHeader!, CancellationToken.None);

        // Assert
        pk.Should().NotBeNull();
        pk!.Name.Should().Be($"PK_{tableName}");
        pk.ColumnNames.Count.Should().Be(2);
        pk.ColumnNames[0].Name.Should().Be("Key1");
        pk.ColumnNames[0].Direction.Should().Be(Direction.Ascending);
        pk.ColumnNames[1].Name.Should().Be("Key2");
        pk.ColumnNames[1].Direction.Should().Be(Direction.Ascending);
    }

    [Fact]
    public async Task TestGetPrimaryKey_When_NotExist()
    {
        // Arrange
        var tableName = GetName();
        await DropTableAsync(tableName);
        await ExecuteNonQueryAsync($"CREATE TABLE [{tableName}]([Key1] int NOT NULL, [Key2] int NOT NULL, [AnotherCol] nvarchar(20))");
        var tableHeader = await MssSystemTables.GetTableHeaderAsync(
            "dbo", tableName, CancellationToken.None);
        tableHeader.Should().NotBeNull();

        // Act
        var pk = await MssSystemTables.GetPrimaryKeyAsync(tableHeader!, CancellationToken.None);

        // Assert
        pk.Should().BeNull($"because {tableName} doesn't have a primary key");
    }
}