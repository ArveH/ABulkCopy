namespace CrossRDBMS.Tests.CopyFromMssToPg;

public class CopyMssToPgBase : TestBase
{
    private readonly IMssCmd _mssCmd;
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _output;
    protected MockFileSystem DummyFileSystem;
    protected string[]? MssArguments;
    protected string[]? PgArguments;

    private IIdentifier? _identifier;

    public CopyMssToPgBase(
        IMssCmd mssCmd,
        DatabaseFixture fixture, 
        ITestOutputHelper output)
    {
        _mssCmd = mssCmd;
        _fixture = fixture;
        _output = output;
        DummyFileSystem  = new MockFileSystem();
    }

    protected async Task TestSingleTypeAsync(
        string tableName,
        IColumn col,
        string insertedValueAsString,
        string expectedPgType)
    {
        List<string> logMessages = new();
        var mssArgs = MssArguments ?? ParamHelper.GetOutMss(_fixture.MssConnectionString, searchFilter: tableName);
        var pgArgs = PgArguments ?? ParamHelper.GetInPg(_fixture.PgConnectionString, searchFilter: $@"\b{tableName}\b");
        var mssContext = new CopyContext(
            Rdbms.Mss,
            CmdArguments.Create(mssArgs),
            logMessages,
            _output,
            DummyFileSystem);
        var pgContext = new CopyContext(
            Rdbms.Pg,
            CmdArguments.Create(pgArgs),
            logMessages,
            _output,
            DummyFileSystem);
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
        await AssertFilesExists(DummyFileSystem, tableName);
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
        await _mssCmd.DropTableAsync(st, CancellationToken.None);
        var tableDef = MssTestData.GetEmpty(st);
        tableDef.Columns.Add(mssCol);
        await _mssCmd.CreateTableAsync(tableDef, CancellationToken.None);
        await _fixture.MssRawCommand.ExecuteNonQueryAsync(
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
    
    protected static MockFileData GetMappingFile(string typeConversions)
    {
        return new MockFileData(
            "{\r\n" +
            "    \"Schemas\": {\r\n" +
            "        \"\": \"public\",\r\n" +
            "        \"dbo\": \"public\",\r\n" +
            "        \"my_mss_schema\": \"my_pg_schema\"\r\n" +
            "    },\r\n" +
            "    \"Collations\": {\r\n" +
            "        \"SQL_Latin1_General_CP1_CI_AI\": \"en_ci_ai\",\r\n" +
            "        \"SQL_Latin1_General_CP1_CI_AS\": \"en_ci_as\"\r\n" +
            "    },\r\n" +
            "  \"ColumnTypes\": {\r\n" +
            typeConversions +
            "  }\r\n" +
            "}");
    }
}