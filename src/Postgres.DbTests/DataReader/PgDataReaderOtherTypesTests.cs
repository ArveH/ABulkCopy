namespace Postgres.DbTests.DataReader;

public class PgDataReaderOtherTypesTests : PgDataReaderTestBase
{
    private const string ColName = "Col1";

    public PgDataReaderOtherTypesTests(ITestOutputHelper output)
        : base(output)
    {
    }

    [Fact]
    public async Task TestUuid()
    {
        // Arrange
        var col = new PostgresUuid(1, ColName, false);
        var colValue = await TestDataReader<Guid>(
            GetName(), col, "8e98618c-6a78-4fa2-9c0e-b6bb2571af72,");

        colValue.Should().Be(new Guid("8e98618c-6a78-4fa2-9c0e-b6bb2571af72"));
    }

    [Fact]
    public async Task TestUuid_When_Null()
    {
        // Arrange
        var col = new PostgresUuid(1, ColName, true);
        var colValue = await TestDataReader<Guid?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task TestByteA()
    {
        // Arrange
        var tableName = GetName();
        var col = new PostgresByteA(1, ColName, true);
        var blobFileName = $"i{0:D15}.raw";
        var blobFilePath = Path.Combine(
            FileHelper.DataFolder, tableName, ColName, blobFileName);
        var fileData = new MockFileData(AllTypes.SampleValues.Binary5K);
        FileHelper.FileSystem.AddFile(blobFilePath, fileData);
        
        var colValue = await TestDataReader<byte[]?>(
            tableName, col, blobFileName + ",");

        colValue.Should().NotBeNull();
        colValue!.Length.Should().Be(5000);
        colValue[65].Should().Be((byte)'A');
    }

    [Fact]
    public async Task TestByteA_When_Null()
    {
        // Arrange
        var col = new PostgresByteA(1, ColName, true);
        var colValue = await TestDataReader<byte[]?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }
}