namespace APostgres.Test.DataReader;

public class PgDataReaderNumberTests : PgDataReaderTestBase, IAsyncLifetime
{
    private const string ColName = "Col1";

    public PgDataReaderNumberTests(ITestOutputHelper output)
        : base(output, nameof(PgDataReaderNumberTests))
    {
    }

    public Task InitializeAsync() => Task.CompletedTask;

    Task IAsyncLifetime.DisposeAsync()
    {
        return PgDbHelper.Instance.DropTable(TestTableName);
    }

    [Fact]
    public async Task TestBigint()
    {
        var col = new PostgresBigInt(1, ColName, false);
        var colValue = await TestDataReader<long>(
            col, AllTypes.SampleValues.BigInt + ",");

        colValue.Should().Be(AllTypes.SampleValues.BigInt);
    }

    [Fact]
    public async Task TestInt()
    {
        // Arrange
        var col = new PostgresInt(1, ColName, false);
        var colValue = await TestDataReader<int>(
            col, AllTypes.SampleValues.Int + ",");

        colValue.Should().Be(AllTypes.SampleValues.Int);
    }

}