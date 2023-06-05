using ABulkCopy.Common.TestData;

namespace APostgres.Test;

public class PgDataReaderNumberTests : PgDataReaderTestBase
{
    public PgDataReaderNumberTests(ITestOutputHelper output)
        : base(output, nameof(PgDataReaderNumberTests))
    {
    }

    [Fact]
    public async Task TestBigint()
    {
        // Arrange
        var col = new PostgresBigInt(1, "Col1", false);
        OriginalTableDefinition.Columns.Add(col);
        await PgDbHelper.Instance.DropTable(TestTableName);
        await PgDbHelper.Instance.CreateTable(OriginalTableDefinition);
        FileHelper.CreateSingleRowDataFile(AllTypes.SampleValues.BigInt.ToString() );

        // Act
        try
        {
            await TestDataReader.Read(
                OriginalTableDefinition,
                TestPath);
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(TestTableName);
        }
    }
}