namespace CrossRDBMS.Tests.CopyFromMssToPg;

[Collection(nameof(DatabaseCollection))]
public class CopyDateTimes(DatabaseFixture fixture, ITestOutputHelper output)
    : CopyMssToPgBase(fixture, output)
{
    [Fact]
    public async Task CopyDateTime()
    {
        var tableName = GetName(nameof(CopyFromMssToPg));
        var colValue = new DateTime(2024, 11, 26, 11, 0, 0);
        var col = new SqlServerDateTime(101, "col1", false);

        await TestSingleTypeAsync(
            tableName, col, "'2024-11-26 11:00:00'", PgTypes.TimestampTz);

        var actualValue = await ValidateValueAsync(("public", tableName), colValue);
        actualValue.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public async Task CopyDateTime2()
    {
        var tableName = GetName(nameof(CopyFromMssToPg));
        var colValue = new DateTime(2024, 11, 26, 11, 0, 0);
        var col = new SqlServerDateTime2(101, "col1", false);

        await TestSingleTypeAsync(
            tableName, col, "'2024-11-26 11:00:00'", PgTypes.TimestampTz);

        var actualValue = await ValidateValueAsync(("public", tableName), colValue);
        actualValue.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public async Task CopyDateTimeOffset()
    {
        var tableName = GetName(nameof(CopyFromMssToPg));
        var colValue = new DateTimeOffset(2024, 11, 26, 11, 0, 0, TimeSpan.Zero);
        var col = new SqlServerDateTimeOffset(101, "col1", false);

        await TestSingleTypeAsync(
            tableName, col, "'2024-11-26 11:00:00Z'", PgTypes.TimestampTz);

        // NOTE: timestamp with time zone is returned as DateTime
        // TODO: Look into using the NodaTime package for Postgres
        var actualValue = await ValidateValueAsync(("public", tableName), colValue.DateTime);
        actualValue.Kind.Should().Be(DateTimeKind.Utc);
    }
}