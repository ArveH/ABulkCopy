namespace SqlServer.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class MssTestGetTableHeader(
    DatabaseFixture dbFixture, ITestOutputHelper output) 
    : MssTestBase(dbFixture, output)
{
    [Fact]
    public async Task TestGetTableHeader_When_NameAndSchemaDbo()
    {
        // Act
        var tableName = GetName();
        var schemaName = "dbo";
        await CreateTableAsync(schemaName, tableName);
        var tableHeader = await MssSystemTables.GetTableHeaderAsync(
            schemaName, tableName, CancellationToken.None);

        // Assert
        tableHeader.Should().NotBeNull();
        tableHeader!.Id.Should().BeGreaterThan(0);
        tableHeader.Name.Should().Be(tableName);
        tableHeader.Schema.Should().Be("dbo");
    }

    [Fact]
    public async Task TestGetTableHeader_When_NameAndTestSchema()
    {
        // Act
        var schemaName = MssDbHelper.TestSchemaName;
        var tableName = GetName();
        await CreateTableAsync(schemaName, tableName);
        var tableHeader = await MssSystemTables.GetTableHeaderAsync(
            schemaName, tableName, CancellationToken.None);

        // Assert
        tableHeader.Should().NotBeNull();
        tableHeader!.Id.Should().BeGreaterThan(0);
        tableHeader.Name.Should().Be(tableName);
        tableHeader.Schema.Should().Be(schemaName);
    }

    [Fact]
    public async Task TestGetTableHeader_When_IdentityOk()
    {
        // Act
        var schemaName = "dbo";
        var tableName = GetName();
        await CreateTableAsync(schemaName, tableName);
        var tableHeader = await MssSystemTables.GetTableHeaderAsync(schemaName, tableName, CancellationToken.None);

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
            $"CREATE TABLE [{schema}].[{tableName}](\r\n\t[Id] [bigint] IDENTITY(1,1) NOT NULL,\r\n\t[ExactNumBigInt] [bigint] NOT NULL)");
    }
}