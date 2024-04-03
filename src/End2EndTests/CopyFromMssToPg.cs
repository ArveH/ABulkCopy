namespace End2EndTests;

[Collection(nameof(DatabaseCollection))]
public class CopyFromMssToPg : End2EndBase, IDisposable
{
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _output;

    public CopyFromMssToPg(DatabaseFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }

    [Fact]
    public async Task CopyInt()
    {
        // Arrange
        var tableName = GetName(nameof(CopyFromMssToPg));
        DeleteFiles(tableName);
        await CreateTableAsync(tableName);
        var mssServices = CopyProg.GetServices(
            CopyDirection.Out, Rdbms.Mss, _fixture.MssConnectionString, tableName);
        var pgServices = CopyProg.GetServices(
            CopyDirection.In, Rdbms.Pg, _fixture.PgConnectionString);
        var copyOut = mssServices.GetRequiredService<ICopyOut>();
        var copyIn = pgServices.GetRequiredService<ICopyIn>();

        // Act
        await copyOut.RunAsync(CancellationToken.None);
        await copyIn.RunAsync(Rdbms.Pg, CancellationToken.None);
        
        // Assert
        var schemaFile = await File.ReadAllTextAsync(tableName + ".schema");
        schemaFile.Should().NotBeNullOrEmpty("because the schema file should exist");
        schemaFile.Should().Contain($"\"Name\": \"{tableName}\"");
        var dataFile = await File.ReadAllTextAsync(tableName + ".data");
        dataFile.Should().NotBeNull("because the data file should exist");
    }

    private async Task CreateTableAsync(string tableName)
    {
        await using var conn = new SqlConnection(_fixture.MssConnectionString);
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = $"CREATE TABLE {tableName} (Id INT PRIMARY KEY, Name NVARCHAR(50))";
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    private static void DeleteFiles(string tableName)
    {
        File.Delete(tableName + ".schema");
        File.Delete(tableName + ".data");
    }

    public void Dispose() => _output.WriteLine(_fixture.MssContainerId);
}