namespace APostgres.Test.DataReader;

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
}