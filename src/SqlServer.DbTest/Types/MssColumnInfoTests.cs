namespace SqlServer.DbTest.Types;

public class MssColumnInfoTests : MssTestBase
{
    private readonly CancellationTokenSource _cts = new();

    public MssColumnInfoTests(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(1, MssTypes.BigInt, 8, 19, 0)]
    [InlineData(2, MssTypes.Int, 4, 10, 0)]
    [InlineData(3, MssTypes.SmallInt, 2, 5, 0)]
    [InlineData(4, MssTypes.TinyInt, 1, 3, 0)]
    [InlineData(5, MssTypes.Bit, 1, 1, 0)]
    [InlineData(6, MssTypes.Money, 8, 19, 4)]
    [InlineData(7, MssTypes.SmallMoney, 4, 10, 4)]
    public async Task TestTypeAndSize_When_FixedSize(
        int colNum,
        string type,
        int length,
        int precision,
        int scale)
    {
        // Arrange
        var tableHeader = await MssSystemTables.GetTableHeaderAsync("AllTypes", _cts.Token);
        tableHeader.Should().NotBeNull();

        // Act
        var columnInfo = (await MssSystemTables.GetTableColumnInfoAsync(tableHeader!, _cts.Token)).ToList();

        // Assert
        columnInfo.Should().NotBeNull();
        columnInfo.Count.Should().Be(28);

        var col = columnInfo[colNum];
        col.Type.Should().Be(type, $"because col {colNum} should be of type {type}");
        col.Length.Should().Be(length, $"because length of {type} should be {length}");
        col.Precision.Should().Be(precision, $"because precision of {type} should be {precision}");
        col.Scale.Should().Be(scale, $"because scale of {type} should be {scale}");
    }
}