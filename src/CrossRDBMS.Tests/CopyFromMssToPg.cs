namespace CrossRDBMS.Tests;

[Collection(nameof(DatabaseCollection))]
public class CopyFromMssToPg : TestBase
{
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _output;
    private IIdentifier? _identifier;

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
        await DropTableAsync(tableName);
        await CreateTableAsync(tableName, "int");
        var mssServices = CopyProg.GetServices(
            CopyDirection.Out, Rdbms.Mss, _fixture.MssConnectionString, tableName);
        var pgServices = CopyProg.GetServices(
            CopyDirection.In, Rdbms.Pg, _fixture.PgConnectionString, $"\\b{tableName}\\b");
        var copyOut = mssServices.GetRequiredService<ICopyOut>();
        var copyIn = pgServices.GetRequiredService<ICopyIn>();
        _identifier = pgServices.GetRequiredService<IIdentifier>();

        // Act
        await copyOut.RunAsync(CancellationToken.None);
        await copyIn.RunAsync(Rdbms.Pg, CancellationToken.None);

        // Assert
        var schemaFile = await File.ReadAllTextAsync(tableName + ".schema");
        schemaFile.Should().NotBeNullOrEmpty("because the schema file should exist");
        schemaFile.Should().Contain($"\"Name\": \"{tableName}\"");
        var dataFile = await File.ReadAllTextAsync(tableName + ".data");
        dataFile.Should().NotBeNull("because the data file should exist");
        await ValidateTypeInfoAsync(tableName, "integer", null, 32, 0);
    }

    private async Task DropTableAsync(string tableName)
    {
        await using var conn = new SqlConnection(_fixture.MssConnectionString);
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = $"DROP TABLE IF EXISTS {tableName}";
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    private async Task CreateTableAsync(string tableName, string colType)
    {
        await using var conn = new SqlConnection(_fixture.MssConnectionString);
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = $"CREATE TABLE {tableName} (col1 {colType})";
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    private async Task ValidateTypeInfoAsync(
        string tableName,
        string expectedType,
        int? expectedLength = null,
        int? expectedPrecision = null,
        int? expectedScale = null)
    {
        var sqlString =
            "select data_type, \r\n " +
            "       character_maximum_length, numeric_precision, numeric_scale, \r\n " +
            "       character_octet_length\r\n " +
            " from information_schema.columns\r\n " +
            $"where table_name = '{_identifier?.AdjustForSystemTable(tableName)}'\r\n " +
            "  and column_name = 'col1'";

        try
        {
            await using var cmd = _fixture.PgContext.DataSource.CreateCommand(sqlString);
            var reader = await cmd.ExecuteReaderAsync();
            var foundColumn = await reader.ReadAsync();
            foundColumn.Should().BeTrue($"because column '{tableName}.col1' should exist");
            VerifyExpectedType(reader, expectedType);
            VerifyExpectedLength(reader, expectedLength);
            VerifyExpectedPrecision(reader, expectedPrecision);
            VerifyExpectedScale(reader, expectedScale);
        }
        catch (Exception ex)
        {
            ex.Message.Should().BeNullOrEmpty("because no exception should be thrown");
        }
    }

    private void VerifyExpectedType(NpgsqlDataReader reader, string expectedType)
    {
        reader.GetString(0).Should().Be(expectedType,
                       $"because the type should be {expectedType}");
    }

    private void VerifyExpectedScale(NpgsqlDataReader reader, int? expectedScale)
    {
        if (expectedScale.HasValue)
        {
            reader.IsDBNull(3).Should().BeFalse("because the column should have a scale");
            reader.GetInt32(3).Should().Be(expectedScale.Value,
                               $"because the scale should be {expectedScale.Value}");
        }
    }

    private void VerifyExpectedPrecision(NpgsqlDataReader reader, int? expectedPrecision)
    {
        if (expectedPrecision.HasValue)
        {
            reader.IsDBNull(2).Should().BeFalse("because the column should have a precision");
            reader.GetInt32(2).Should().Be(expectedPrecision.Value,
                               $"because the precision should be {expectedPrecision.Value}");
        }
    }

    private void VerifyExpectedLength(NpgsqlDataReader reader, int? expectedLength)
    {
        if (expectedLength.HasValue)
        {
            reader.IsDBNull(1).Should().BeFalse("because the column should have a length");
            reader.GetInt32(1).Should().Be(expectedLength.Value,
                $"because the length should be {expectedLength.Value}");
        }
    }

    private static void DeleteFiles(string tableName)
    {
        File.Delete(tableName + ".schema");
        File.Delete(tableName + ".data");
    }
}