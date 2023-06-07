namespace APostgres.Test.DataReader;

public class PgDataReaderDateTimeTests : PgDataReaderTestBase
{
    private const string ColName = "Col1";

    public PgDataReaderDateTimeTests(ITestOutputHelper output)
        : base(output, nameof(PgDataReaderDateTimeTests))
    {
    }

    [Fact]
    public async Task TestDate()
    {
        // Arrange
        var testVal = new DateOnly(2023, 06, 14);
        var col = new PostgresDate(1, ColName, false);
        var colValue = await TestDataReader<DateOnly>(
            col, "2023-06-14,");

        colValue.Should().Be(testVal);
    }


}