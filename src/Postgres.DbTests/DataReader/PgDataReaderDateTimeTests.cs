namespace Postgres.DbTests.DataReader;

public class PgDataReaderDateTimeTests : PgDataReaderTestBase
{
    private const string ColName = "Col1";

    public PgDataReaderDateTimeTests(ITestOutputHelper output)
        : base(output)
    {
    }

    [Fact]
    public async Task TestDate()
    {
        // Arrange
        var testVal = new DateOnly(2023, 06, 14);
        var col = new PostgresDate(1, ColName, false);
        var colValue = await TestDataReader<DateOnly>(
            GetName(), col, "2023-06-14,");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestDate_When_Null()
    {
        // Arrange
        var col = new PostgresDate(1, ColName, true);
        var colValue = await TestDataReader<DateOnly?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task? TestTime_When_NoFractions()
    {
        // Arrange
        var testVal = new TimeOnly(11, 12, 13);
        var col = new PostgresTime(1, ColName, false);
        var colValue = await TestDataReader<TimeOnly>(
            GetName(), col, "11:12:13,");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task? TestTime_When_Fractions()
    {
        // Arrange
        var testVal = new TimeOnly(11, 12, 13, 456, 789);
        var col = new PostgresTime(1, ColName, false);
        var colValue = await TestDataReader<TimeOnly>(
            GetName(), col, "11:12:13.456789,");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task? TestTime_When_FractionsRounded()
    {
        // Arrange
        var testVal = new TimeOnly(11, 12, 13, 456, 789);
        var col = new PostgresTime(1, ColName, false, 2);
        var colValue = await TestDataReader<TimeOnly>(
            GetName(), col, "11:12:13.456789,");

        colValue.Should().Be(new TimeOnly(11, 12, 13, 460, 0));
    }

    [Fact]
    public async Task TestTime_When_Null()
    {
        // Arrange
        var col = new PostgresTime(1, ColName, true);
        var colValue = await TestDataReader<TimeOnly?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task? TestTimestamp_When_NoFractions()
    {
        // Arrange
        var testVal = new DateTime(2023, 6, 23, 11, 12, 13);
        var col = new PostgresTimestamp(1, ColName, false);
        var colValue = await TestDataReader<DateTime>(
            GetName(), col, "2023-06-23T11:12:13,");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task? TestTimestamp_When_Fractions()
    {
        // Arrange
        var testVal = new DateTime(2023, 6, 23, 11, 12, 13, 456, 789);
        var col = new PostgresTimestamp(1, ColName, false);
        var colValue = await TestDataReader<DateTime>(
            GetName(), col, "2023-06-23T11:12:13.456789,");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestTimestamp_When_Null()
    {
        // Arrange
        var col = new PostgresTimestamp(1, ColName, true);
        var colValue = await TestDataReader<DateTime?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task TestTimestamp_When_MinDate()
    {
        // Arrange
        var col = new PostgresTimestamp(1, ColName, true);
        var colValue = await TestDataReader<DateTime?>(
            GetName(), col, "0001-01-01T00:00:00.0000000,");

        colValue.Should().Be(DateTime.MinValue);
    }

    [Fact]
    public async Task? TestTimestampTz_When_NoFractions()
    {
        // Arrange
        var col = new PostgresTimestampTz(1, ColName, false);
        var colValue = await TestDataReader<DateTimeOffset>(
            GetName(), col, "2023-06-23T08:00:00-02:00,");

        colValue.Should().Be(
            new DateTimeOffset(2023, 6, 23, 8, 0, 0, TimeSpan.FromHours(-2)));
    }

    [Fact]
    public async Task? TestTimestampTz_When_Fractions()
    {
        // Arrange
        var col = new PostgresTimestampTz(1, ColName, false);
        var colValue = await TestDataReader<DateTimeOffset>(
            GetName(), col, "2023-06-23T08:10:11.456789 +1:30,");

        colValue.Should().Be(
            new DateTimeOffset(2023, 6, 23, 8, 10, 11, 456, 789, TimeSpan.FromMinutes(90)));
    }

    [Fact]
    public async Task TestTimestampTz_When_Null()
    {
        // Arrange
        var col = new PostgresTimestampTz(1, ColName, true);
        var colValue = await TestDataReader<DateTimeOffset?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }
}