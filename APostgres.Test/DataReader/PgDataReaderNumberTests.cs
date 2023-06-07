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

    [Fact]
    public async Task TestSmallInt()
    {
        // Arrange
        var col = new PostgresSmallInt(1, ColName, false);
        var colValue = await TestDataReader<int>(
            col, AllTypes.SampleValues.SmallInt + ",");

        colValue.Should().Be(AllTypes.SampleValues.SmallInt);
    }

    [Fact]
    public async Task TestBoolean_When_1()
    {
        // Arrange
        var col = new PostgresBoolean(1, ColName, false);
        var colValue = await TestDataReader<bool>(
            col, "1,");

        colValue.Should().BeTrue();
    }

    [Fact]
    public async Task TestBoolean_When_0()
    {
        // Arrange
        var col = new PostgresBoolean(1, ColName, false);
        var colValue = await TestDataReader<bool>(
            col, "0,");

        colValue.Should().BeFalse();
    }

    [Fact]
    public async Task TestBoolean_When_true()
    {
        // Arrange
        var col = new PostgresBoolean(1, ColName, false);
        var colValue = await TestDataReader<bool>(
            col, "true,");

        colValue.Should().BeTrue();
    }

    [Fact]
    public async Task TestBoolean_When_false()
    {
        // Arrange
        var col = new PostgresBoolean(1, ColName, false);
        var colValue = await TestDataReader<bool>(
            col, "false,");

        colValue.Should().BeFalse();
    }

    [Fact]
    public async Task TestLargeDecimal()
    {
        // Arrange
        var testVal = 12345678901234567890.123456m;
        var col = new PostgresDecimal(1, ColName, false, 32, 6);
        var colValue = await TestDataReader<decimal>(
            col, testVal + ",");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestSmallDecimal()
    {
        // Arrange
        var testVal = 1234m;
        var col = new PostgresDecimal(1, ColName, false, 4);
        var colValue = await TestDataReader<decimal>(
            col, testVal + ",");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestDoublePrecision()
    {
        // Arrange
        var testVal = 12345678.123456d;
        var col = new PostgresDoublePrecision(1, ColName, false);
        var colValue = await TestDataReader<double>(
            col, testVal + ",");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestMoney()
    {
        // Arrange
        var testVal = 123.12m;
        var col = new PostgresMoney(1, ColName, false);
        var colValue = await TestDataReader<decimal>(
            col, testVal + ",");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestReal()
    {
        // Arrange
        var testVal = 123.12f;
        var col = new PostgresReal(1, ColName, false);
        var colValue = await TestDataReader<float>(
            col, testVal + ",");

        colValue.Should().Be(testVal);
    }
}