namespace Postgres.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class PgTestGetTableHeader : PgTestBase
{
    public PgTestGetTableHeader(DatabaseFixture dbFixture, ITestOutputHelper output) 
        : base(dbFixture, output)
    {
    }
    
    [Fact]
    public async Task TestGetTableHeader_When_NameAndSchemaDbo()
    {
        // Act
        var tableName = GetName();
        await CreateTableAsync(DatabaseFixture.DefaultSchemaName, tableName);
        var tableHeader = await PgSystemTables.GetTableHeaderAsync(
            DatabaseFixture.DefaultSchemaName, tableName, CancellationToken.None);

        // Assert
        tableHeader.Should().NotBeNull();
        tableHeader!.Id.Should().BeGreaterThan(0);
        tableHeader.Name.Should().Be(tableName);
        tableHeader.Schema.Should().Be(DatabaseFixture.DefaultSchemaName);
    }

    [Fact]
    public async Task TestGetTableHeader_When_NameAndTestSchema()
    {
        // Act
        var tableName = GetName();
        await CreateTableAsync(DatabaseFixture.TestSchemaName, tableName);
        var tableHeader = await PgSystemTables.GetTableHeaderAsync(
            DatabaseFixture.TestSchemaName, tableName, CancellationToken.None);

        // Assert
        tableHeader.Should().NotBeNull();
        tableHeader!.Id.Should().BeGreaterThan(0);
        tableHeader.Name.Should().Be(tableName);
        tableHeader.Schema.Should().Be(DatabaseFixture.TestSchemaName);
    }

    [Fact]
    public async Task TestGetTableHeader_When_IdentityOk()
    {
        // Act
        var tableName = GetName();
        await CreateTableAsync(DatabaseFixture.DefaultSchemaName, tableName);
        var tableHeader = await PgSystemTables.GetTableHeaderAsync(
            DatabaseFixture.DefaultSchemaName, tableName, CancellationToken.None);

        // Assert
        tableHeader.Should().NotBeNull();
        tableHeader!.Identity.Should().NotBeNull();
        tableHeader.Identity!.Seed.Should().Be(1);
        tableHeader.Identity.Increment.Should().Be(1);
    }

    private async Task CreateTableAsync(string schema, string tableName)
    {
        await DropTableAsync(schema, tableName);
        await ExecuteNonQueryAsync(
            $"CREATE TABLE {schema}.{tableName}(" + Environment.NewLine +
            "\tId bigint GENERATED ALWAYS AS IDENTITY NOT NULL," + Environment.NewLine +
            "\tExactNumBigInt bigint NOT NULL)");
    }
}