namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestByteA : PgColumnTypeTestBase
{
    // Must match the raw file name convention in DataWriter
    private static readonly string RawFileName = $"i{0:D15}.raw";

    public TestByteA(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestByteA_When_BasicValue()
    {
        await TestByteARoundTrip(GetName(), AllTypes.SampleValues.Binary5K);
    }

    [Fact]
    public async Task TestByteA_When_SingleByte()
    {
        await TestByteARoundTrip(GetName(), new byte[] { 0xFF });
    }

    [Fact]
    public async Task TestByteA_When_Empty()
    {
        await TestByteARoundTrip(GetName(), []);
    }

    [Fact]
    public async Task TestByteA_When_Null()
    {
        await TestColumnRoundTrip<byte[]>(
            GetName(),
            new PostgresByteA(1, ColName, true),
            null,
            null);
    }

    private async Task TestByteARoundTrip(string tableName, byte[] value)
    {
        // Arrange: bytea values live in separate raw files, the data file
        // only holds the raw file name
        SchemaTableTuple st = (DatabaseFixture.DefaultSchemaName, tableName);
        var col = new PostgresByteA(1, ColName, true);
        var inBlobPath = Path.Combine(
            FileHelper.DataFolder, st.GetFullName(), ColName, RawFileName);
        FileHelper.FileSystem.AddFile(inBlobPath, new MockFileData(value));
        var tableDefinition = await CreateTableAndDataFileAsync(
            st, col, [ToRow(RawFileName)]);
        try
        {
            // Act (copy in)
            var rowsRead = await CopyInAsync(tableDefinition);

            // Assert
            rowsRead.Should().Be(1);
            var dbValue = await DbFixture.SelectScalar<byte[]>(st, col);
            dbValue.Should().Equal(value);

            // Act (copy out)
            var outText = await CopyOutAsync(tableDefinition);

            // Assert
            outText.TrimEnd('\r', '\n').Should().Be(ToRow(RawFileName));
            var outBlobPath = Path.Combine(
                OutFolder, st.GetFullName(), ColName, RawFileName);
            FileHelper.FileSystem.FileExists(outBlobPath).Should().BeTrue();
            FileHelper.FileSystem.File.ReadAllBytes(outBlobPath).Should().Equal(value);
        }
        finally
        {
            await DbFixture.DropTable(st);
        }
    }
}
