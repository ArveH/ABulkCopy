namespace CrossRDBMS.Tests.CopyFromMssToPg;

[Collection(nameof(DatabaseCollection))]
public class CopyDateTimes : CopyMssToPgBase
{
    public CopyDateTimes(DatabaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        PgArguments = ParamHelper.GetInPg(
            fixture.PgConnectionString,
            mappingsFile: "mymappings.json");
    }

    [Fact]
    public async Task CopyDateTime_To_TimeStampTz()
    {
        var tableName = GetName(nameof(CopyFromMssToPg));
        var colValue = new DateTime(2024, 11, 26, 11, 0, 0);
        var col = new SqlServerDateTime(101, "col1", false);
        MapAllDateTimesToTimestampTz();

        await TestSingleTypeAsync(
            tableName, col, "'2024-11-26 11:00:00'", PgTypes.TimestampTz);

        var actualValue = await ValidateValueAsync(("public", tableName), colValue);
        actualValue.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public async Task CopyDateTime2_To_TimeStampTz()
    {
        var tableName = GetName(nameof(CopyFromMssToPg));
        var colValue = new DateTime(2024, 11, 26, 11, 0, 0);
        var col = new SqlServerDateTime2(101, "col1", false);
        MapAllDateTimesToTimestampTz();

        await TestSingleTypeAsync(
            tableName, col, "'2024-11-26 11:00:00'", PgTypes.TimestampTz);

        var actualValue = await ValidateValueAsync(("public", tableName), colValue);
        actualValue.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public async Task CopyDateTime_To_TimeStamp()
    {
        var tableName = GetName(nameof(CopyFromMssToPg));
        var colValue = new DateTime(2024, 11, 26, 11, 0, 0);
        var col = new SqlServerDateTime(101, "col1", false);
        MapOnlyDateTimeOffsetToTimestampTz();

        await TestSingleTypeAsync(
            tableName, col, "'2024-11-26 11:00:00'", "timestamp without time zone");

        var actualValue = await ValidateValueAsync(("public", tableName), colValue);
        actualValue.Kind.Should().Be(DateTimeKind.Unspecified);
    }

    [Fact]
    public async Task CopyDateTime2_To_TimeStamp()
    {
        var tableName = GetName(nameof(CopyFromMssToPg));
        var colValue = new DateTime(2024, 11, 26, 11, 0, 0);
        var col = new SqlServerDateTime2(101, "col1", false);
        MapOnlyDateTimeOffsetToTimestampTz();

        await TestSingleTypeAsync(
            tableName, col, "'2024-11-26 11:00:00'", "timestamp without time zone");

        var actualValue = await ValidateValueAsync(("public", tableName), colValue);
        actualValue.Kind.Should().Be(DateTimeKind.Unspecified);
    }

    [Fact]
    public async Task CopyDateTimeOffset_To_TimeStampTz()
    {
        var tableName = GetName(nameof(CopyFromMssToPg));
        var colValue = new DateTimeOffset(2024, 11, 26, 11, 0, 0, TimeSpan.Zero);
        var col = new SqlServerDateTimeOffset(101, "col1", false);
        MapAllDateTimesToTimestampTz();

        await TestSingleTypeAsync(
            tableName, col, "'2024-11-26 11:00:00Z'", PgTypes.TimestampTz);

        // NOTE: timestamp with time zone is returned as DateTime
        // TODO: Look into using the NodaTime package for Postgres
        var actualValue = await ValidateValueAsync(("public", tableName), colValue.DateTime);
        actualValue.Kind.Should().Be(DateTimeKind.Utc);
    }

    private void MapOnlyDateTimeOffsetToTimestampTz()
    {
        DummyFileSystem.AddFile(@"c:\mymappings.json", GetMappingFile(
            "    \"datetime\": \"timestamp\",\r\n" +
            "    \"datetime2\": \"timestamp\",\r\n" +
            "    \"datetimeoffset\": \"timestamp with time zone\"\r\n"));
    }

    private void MapAllDateTimesToTimestampTz()
    {
        DummyFileSystem.AddFile(@"c:\mymappings.json", GetMappingFile(
            "    \"datetime\": \"timestamp with time zone\",\r\n" +
            "    \"datetime2\": \"timestamp with time zone\",\r\n" +
            "    \"datetimeoffset\": \"timestamp with time zone\"\r\n"));
    }

    private static MockFileData GetMappingFile(string typeConversions)
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