namespace CrossRDBMS.Tests.CopyFromMssToPg;

public class CopyMssToPgBase : TestBase
{
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _output;

    private IIdentifier? _identifier;

    public CopyMssToPgBase(DatabaseFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }

    protected async Task TestSingleTypeAsync(
        string tableName,
        IColumn col,
        string insertedValueAsString,
        string expectedPgType)
    {
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
        await CreateTableAsync(
            ("dbo", tableName),
            col,
            insertedValueAsString);

        var copyOut = mssContext.GetServices<ICopyOut>();
        var copyIn = pgContext.GetServices<ICopyIn>();
        _identifier = pgContext.GetServices<IIdentifier>();

        // Act
        await copyOut.RunAsync(CancellationToken.None);
        await copyIn.RunAsync(Rdbms.Pg, CancellationToken.None);

        // Assert
        await AssertFilesExists(fileSystem, tableName);
        await ValidateTypeInfoAsync(tableName, expectedPgType);
    }

    protected async Task<T> ValidateValueAsync<T>(SchemaTableTuple st, T expected)
    {
        await using var cmd = _fixture.PgContext.DataSource.CreateCommand(
            $"select col1 from {st.GetFullName()}");
        var actual = (T?)await cmd.ExecuteScalarAsync();
        actual.Should().NotBeNull("because we don't expect a null value");
        actual.Should().Be(expected);
        return actual!;
    }

    protected static async Task AssertFilesExists(IFileSystem fileSystem, string tableName)
    {
        var schemaFile = await fileSystem.File.ReadAllTextAsync("dbo." + tableName + ".schema");
        schemaFile.Should().NotBeNullOrEmpty("because the schema file should exist");
        schemaFile.Should().Contain($"\"Name\": \"{tableName}\"");
        var dataFile = await fileSystem.File.ReadAllTextAsync("dbo." + tableName + ".data");
        dataFile.Should().NotBeNull("because the data file should exist");
    }

    protected async Task CreateTableAsync(
        SchemaTableTuple st,
        IColumn mssCol,
        string insertedValueAsString)
    {
        await _fixture.MssDbHelper.DropTableAsync(st);
        var tableDef = MssTestData.GetEmpty(st);
        tableDef.Columns.Add(mssCol);
        await _fixture.MssDbHelper.CreateTableAsync(tableDef);
        await _fixture.MssDbHelper.ExecuteNonQueryAsync(
            $"insert into {st.GetFullName()}(col1) values ({insertedValueAsString})",
            CancellationToken.None);
    }

    protected async Task ValidateTypeInfoAsync(
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

    protected void VerifyExpectedType(NpgsqlDataReader reader, string expectedType)
    {
        reader.GetString(0).Should().Be(expectedType,
            $"because the type should be {expectedType}");
    }

    protected void VerifyExpectedScale(NpgsqlDataReader reader, int? expectedScale)
    {
        if (expectedScale.HasValue)
        {
            reader.IsDBNull(3).Should().BeFalse("because the column should have a scale");
            reader.GetInt32(3).Should().Be(expectedScale.Value,
                $"because the scale should be {expectedScale.Value}");
        }
    }

    protected void VerifyExpectedPrecision(NpgsqlDataReader reader, int? expectedPrecision)
    {
        if (expectedPrecision.HasValue)
        {
            reader.IsDBNull(2).Should().BeFalse("because the column should have a precision");
            reader.GetInt32(2).Should().Be(expectedPrecision.Value,
                $"because the precision should be {expectedPrecision.Value}");
        }
    }

    protected void VerifyExpectedLength(NpgsqlDataReader reader, int? expectedLength)
    {
        if (expectedLength.HasValue)
        {
            reader.IsDBNull(1).Should().BeFalse("because the column should have a length");
            reader.GetInt32(1).Should().Be(expectedLength.Value,
                $"because the length should be {expectedLength.Value}");
        }
    }
}