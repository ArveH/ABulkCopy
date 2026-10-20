namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestUuid : PgColumnTypeTestBase
{
    public TestUuid(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestUuid_When_BasicValue()
    {
        await TestColumnRoundTrip(
            GetName(),
            new PostgresUuid(1, ColName, true),
            "8e98618c-6a78-4fa2-9c0e-b6bb2571af72",
            new Guid("8e98618c-6a78-4fa2-9c0e-b6bb2571af72"));
    }

    [Fact]
    public async Task TestUuid_When_UpperCase_Then_WrittenAsLowerCase()
    {
        // Guid.ToString() always writes lower case on copy out
        await TestColumnRoundTrip(
            GetName(),
            new PostgresUuid(1, ColName, true),
            "A17542D9-A61C-4E4C-8512-DAFFC1416142",
            AllTypes.SampleValues.Guid,
            expectedOutputCell: "a17542d9-a61c-4e4c-8512-daffc1416142");
    }

    [Fact]
    public async Task TestUuid_When_EmptyGuid()
    {
        await TestColumnRoundTrip(
            GetName(),
            new PostgresUuid(1, ColName, true),
            "00000000-0000-0000-0000-000000000000",
            Guid.Empty);
    }

    [Fact]
    public async Task TestUuid_When_Null()
    {
        await TestColumnRoundTrip<Guid?>(
            GetName(),
            new PostgresUuid(1, ColName, true),
            null,
            null);
    }
}
