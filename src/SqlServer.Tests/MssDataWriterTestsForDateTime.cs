namespace SqlServer.Tests;

[Collection(nameof(DatabaseCollection))]
public class MssDataWriterTestsForDateTime : MssDataWriterTestBase
{
    public MssDataWriterTestsForDateTime(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output, Environment.MachineName + "MssDataWriterTestsForDateTime")
    {
    }

    [Fact]
    public async Task TestWriteDate()
    {
        var jsonTxt = await ArrangeAndAct(
            new SqlServerDate(101, "MyTestCol", false),
            new DateTime(2023, 5, 19));

        // Assert
        jsonTxt.TrimEnd().Should().Be("2023-05-19,");
    }

    [Fact]
    public async Task TestWriteDateTime_When_NoFraction()
    {
        var testDate = new DateTime(2023, 5, 19, 11, 12, 13, DateTimeKind.Utc);
        var jsonTxt = await ArrangeAndAct(
            new SqlServerDateTime(101, "MyTestCol", false),
            testDate);

        // Assert
        jsonTxt.TrimEnd().Should().Be($"{testDate:O},");
    }

    [Fact]
    public async Task TestWriteDateTime()
    {
        var testDate = new DateTime(2023, 5, 19, 11, 12, 13, 233, DateTimeKind.Utc);
        var jsonTxt = await ArrangeAndAct(
            new SqlServerDateTime(101, "MyTestCol", false),
            testDate);

        // Assert
        jsonTxt.TrimEnd().Should().Be($"{testDate:O},");
    }

    [Fact]
    public async Task TestWriteDateTime2()
    {
        var value = new DateTime(2023, 5, 19, 11, 12, 13, 55, DateTimeKind.Utc);

        var jsonTxt = await ArrangeAndAct(
            new SqlServerDateTime2(101, "MyTestCol", false),
            value,
            SqlDbType.DateTime2);

        // Assert
        jsonTxt.TrimEnd().Should().Be($"{value:O},");
    }

    [Fact]
    public async Task TestWriteDateTime2_When_NanoSeconds()
    {
        var value = new DateTime(2023, 5, 19, 11, 12, 13, 233, 666, DateTimeKind.Utc).AddTicks(8);

        var jsonTxt = await ArrangeAndAct(
            new SqlServerDateTime2(101, "MyTestCol", false),
            value,
            SqlDbType.DateTime2);

        // Assert
        jsonTxt.TrimEnd().Should().Be($"{value:O},");
    }

    [Fact]
    public async Task TestWriteDateTimeOffset()
    {
        var value = new DateTimeOffset(2023, 5, 19, 11, 12, 13, 233, 666, new TimeSpan(1, 0, 0)).AddTicks(8);

        var jsonTxt = await ArrangeAndAct(
            new SqlServerDateTimeOffset(101, "MyTestCol", false),
            value,
            SqlDbType.DateTimeOffset);

        // Assert
        jsonTxt.TrimEnd().Should().Be($"{value:O}Z,");
    }

    [Fact]
    public async Task TestWriteTime()
    {
        var value = new TimeSpan(11, 12, 13);

        var jsonTxt = await ArrangeAndAct(
            new SqlServerTime(101, "MyTestCol", false),
            value,
            SqlDbType.Time);

        // Assert
        jsonTxt.TrimEnd().Should().Be("11:12:13,");
    }
}