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
        // Arrange
        var col = new PostgresBigInt(1, ColName, false);
        TableDefinition.Columns.Add(col);
        await PgDbHelper.Instance.DropTable(TestTableName);
        await PgDbHelper.Instance.CreateTable(TableDefinition);
        FileHelper.CreateDataFile(AllTypes.SampleValues.BigInt + ",");

        // Act
        await TestDataReader.Read(FileHelper.DataFolder, TableDefinition);

        // Assert
        var colValue = await PgDbHelper.Instance.SelectScalar<long>(
            TestTableName, ColName);
        colValue.Should().Be(AllTypes.SampleValues.BigInt);
    }

    [Fact]
    public async Task TestInt()
    {
        // Arrange
        var col = new PostgresInt(1, ColName, false);
        TableDefinition.Columns.Add(col);
        await PgDbHelper.Instance.DropTable(TestTableName);
        await PgDbHelper.Instance.CreateTable(TableDefinition);
        FileHelper.CreateDataFile(AllTypes.SampleValues.Int + ",");

        // Act
        await TestDataReader.Read(FileHelper.DataFolder, TableDefinition);

        // Assert
        var colValue = await PgDbHelper.Instance.SelectScalar<int>(
            TestTableName, ColName);
        colValue.Should().Be(AllTypes.SampleValues.Int);
    }

}