namespace APostgres.Test.DataReader;

public class PgDataReaderDateOtherTypesTests : PgDataReaderTestBase
{
    private const string ColName = "Col1";

    public PgDataReaderDateOtherTypesTests(ITestOutputHelper output)
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
}