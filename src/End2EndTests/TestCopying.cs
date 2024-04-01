namespace End2EndTests;

[Collection(nameof(DatabaseCollection))]
public class TestCopying : IDisposable
{
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _output;

    public TestCopying(DatabaseFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }

    [Fact]
    public async Task TestCopyOutFromMss()
    {
        const string tableName = "AllTypes";
        File.Delete(tableName + ".schema");
        File.Delete(tableName + ".data");

        await using var conn = new SqlConnection(_fixture.ConnectionString);
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = $"CREATE TABLE {tableName} (Id INT PRIMARY KEY, Name NVARCHAR(50))";
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
        
        var copyOut = _fixture.MssServiceProvider.GetRequiredService<ICopyOut>();
        await copyOut.RunAsync(CancellationToken.None);
        
        var schemaFile = await File.ReadAllTextAsync(tableName + ".schema");
        schemaFile.Should().NotBeNullOrEmpty("because the schema file should exist");
        schemaFile.Should().Contain("\"Name\": \"AllTypes\"");
        var dataFile = await File.ReadAllTextAsync(tableName + ".data");
        dataFile.Should().NotBeNull("because the data file should exist");
    }

    public void Dispose() => _output.WriteLine(_fixture.ContainerId);
}