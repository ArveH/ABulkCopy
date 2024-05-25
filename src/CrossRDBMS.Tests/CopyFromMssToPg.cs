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
        List<string> logMessages = new();
        IFileSystem fileSystem = new MockFileSystem();
        var mssContext = new CopyContext(
            Rdbms.Mss,
            CmdArguments.Create(ParamHelper.GetOutMss(
                _fixture.MssConnectionString,
                searchFilter: tableName)),
            logMessages,
            _output,
            fileSystem);
        var pgContext = new CopyContext(
            Rdbms.Pg,
            CmdArguments.Create(ParamHelper.GetInPg(
                _fixture.PgConnectionString,
                searchFilter: $@"\b{tableName}\b")),
            logMessages,
            _output,
            fileSystem);

        await CreateTableAsync(tableName);

        var copyOut = mssContext.GetServices<ICopyOut>();
        var copyIn = pgContext.GetServices<ICopyIn>();
        _identifier = pgContext.GetServices<IIdentifier>();

        // Act
        await copyOut.RunAsync(CancellationToken.None);
        await copyIn.RunAsync(Rdbms.Pg, CancellationToken.None);

        // Assert
        await AssertFilesExists(fileSystem, tableName);
        await ValidateTypeInfoAsync(tableName, "integer", null, 32, 0);
        // TODO: Validate Postgres column and data
    }

    private static async Task AssertFilesExists(IFileSystem fileSystem, string tableName)
    {
        var schemaFile = await fileSystem.File.ReadAllTextAsync("dbo." + tableName + ".schema");
        schemaFile.Should().NotBeNullOrEmpty("because the schema file should exist");
        schemaFile.Should().Contain($"\"Name\": \"{tableName}\"");
        var dataFile = await fileSystem.File.ReadAllTextAsync("dbo." + tableName + ".data");
        dataFile.Should().NotBeNull("because the data file should exist");
    }

    private async Task CreateTableAsync(string tableName)
    {
        await _fixture.DbHelper.DropTableAsync(("dbo", tableName));
        var tableDef = MssTestData.GetEmpty(("dbo", tableName));
        var col = new SqlServerInt(101, "col1", false);
        tableDef.Columns.Add(col);
        await _fixture.DbHelper.CreateTableAsync(tableDef);
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
}