namespace APostgres.Test;

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
        OriginalTableDefinition.Columns.Add(col);
        await PgDbHelper.Instance.DropTable(TestTableName);
        await PgDbHelper.Instance.CreateTable(OriginalTableDefinition);
        FileHelper.CreateSingleRowDataFile(AllTypes.SampleValues.BigInt.ToString());

        // Act
        await TestDataReader.Read(OriginalTableDefinition, TestPath);

        // Assert
        var colValue = await PgDbHelper.Instance.SelectScalar<long>(
            TestTableName, ColName);
        colValue.Should().Be(AllTypes.SampleValues.BigInt);
    }

}