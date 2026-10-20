namespace Postgres.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class PgTestPrimaryKey : PgTestBase
{
    private readonly IIdentifier _identifier;
    public PgTestPrimaryKey(DatabaseFixture dbFixture, ITestOutputHelper output) 
        : base(dbFixture, output)
    {
        _identifier = GetIdentifier();
    }
    
    [Fact]
    public async Task TestGetPrimaryKey_When_Exists()
    {
        // Arrange
        var tableName = GetName();
        await DropTableAsync(DatabaseFixture.DefaultSchemaName, tableName);
        await ExecuteNonQueryAsync(
            $"CREATE TABLE {tableName}(Key1 int NOT NULL, Key2 int NOT NULL, AnotherCol varchar(20), " + Environment.NewLine +
            "PRIMARY KEY (Key1, Key2))");

        var tableHeader = await PgSystemTables.GetTableHeaderAsync(
            DatabaseFixture.DefaultSchemaName, tableName, CancellationToken.None);
        tableHeader.Should().NotBeNull();

        // Act
        var pk = await PgSystemTables.GetPrimaryKeyAsync(tableHeader!, CancellationToken.None);

        // Assert
        pk.Should().NotBeNull();
        pk!.ColumnNames.Count.Should().Be(2);
        pk.ColumnNames[0].Name.Should().Be(_identifier.AdjustForSystemTable("Key1"));
        pk.ColumnNames[0].Direction.Should().Be(Direction.Ascending);
        pk.ColumnNames[1].Name.Should().Be(_identifier.AdjustForSystemTable("Key2"));
        pk.ColumnNames[1].Direction.Should().Be(Direction.Ascending);
    }
    
    [Fact]
    public async Task TestGetPrimaryKey_When_NotExist()
    {
        // Arrange
        var tableName = GetName();
        await DropTableAsync(DatabaseFixture.DefaultSchemaName, tableName);
        await ExecuteNonQueryAsync($"CREATE TABLE {tableName}(Key1 int NOT NULL, Key2 int NOT NULL, AnotherCol varchar(20))");
        var tableHeader = await PgSystemTables.GetTableHeaderAsync(
            DatabaseFixture.DefaultSchemaName, tableName, CancellationToken.None);
        tableHeader.Should().NotBeNull();

        // Act
        var pk = await PgSystemTables.GetPrimaryKeyAsync(tableHeader!, CancellationToken.None);

        // Assert
        pk.Should().BeNull($"because {tableName} doesn't have a primary key");
    }
}