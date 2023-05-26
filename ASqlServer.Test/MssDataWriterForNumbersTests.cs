namespace ASqlServer.Test;

public class MssDataWriterForNumbersTests : MssDataWriterTestBase
{
    public override string _testTableName => Environment.MachineName + "MssDataWriterForNumbersTests";

    public MssDataWriterForNumbersTests(ITestOutputHelper output)
        : base(output)
    {
    }

    [Fact]
    public async Task TestWriteBigInt()
    {
        await TestWriteUsingType(
            new SqlServerBigInt(101, "MyTestCol", false),
            AllTypes.SampleValues.BigInt);
    }

    [Fact]
    public async Task TestWriteInt()
    {
        await TestWriteUsingType(
            new SqlServerInt(101, "MyTestCol", false),
            AllTypes.SampleValues.Int);
    }

    [Fact]
    public async Task TestWriteSmallInt()
    {
        await TestWriteUsingType(
            new SqlServerSmallInt(101, "MyTestCol", false),
            AllTypes.SampleValues.SmallInt);
    }

    [Fact]
    public async Task TestWriteTinyInt()
    {
        await TestWriteUsingType(
            new SqlServerTinyInt(101, "MyTestCol", false),
            AllTypes.SampleValues.TinyInt);
    }

    [Fact]
    public async Task TestWriteBit()
    {
        var jsonTxt = await ArrangeAndAct(
            new SqlServerBit(101, "MyTestCol", false),
            true);
        jsonTxt.TrimEnd().Should().Be("1,");
    }

    [Fact]
    public async Task TestWriteMoney()
    {
        await TestWriteUsingType(
            new SqlServerMoney(101, "MyTestCol", false),
            AllTypes.SampleValues.Money);
    }

    [Fact]
    public async Task TestWriteDecimal()
    {
        await TestWriteUsingType(
            new SqlServerDecimal(101, "MyTestCol", false, 28, 6),
            12345678901234567890.123456m);
    }

    [Fact]
    public async Task TestWriteDecimal_When_Scale0_Then_ValueTruncated()
    {
        var jsonTxt = await ArrangeAndAct(
            new SqlServerDecimal(101, "MyTestCol", false, 28),
            1234567.123);
        jsonTxt.TrimEnd().Should().Be("1234567,");
    }

    [Fact]
    public async Task TestWriteFloat()
    {
        await TestWriteUsingType(
            new SqlServerFloat(101, "MyTestCol", false),
            AllTypes.SampleValues.Float);
    }

    [Fact]
    public async Task TestWriteReal()
    {
        await TestWriteUsingType(
            new SqlServerReal(101, "MyTestCol", false),
            12.25);
    }
}