namespace SqlServer.DbTest.Types;

public class MssColumnInfoTests : MssSystemTablesTestBase
{
    private readonly string _tableName;

    public MssColumnInfoTests(ITestOutputHelper output) : base(output)
    {
        _tableName = GetName();
    }

    [Theory]
    [InlineData("bigint", MssTypes.BigInt, 8, 19, 0)]
    [InlineData("int", MssTypes.Int, 4, 10, 0)]
    [InlineData("smallint", MssTypes.SmallInt, 2, 5, 0)]
    [InlineData("tinyint", MssTypes.TinyInt, 1, 3, 0)]
    [InlineData("bit", MssTypes.Bit, 1, 1, 0)]
    [InlineData("money", MssTypes.Money, 8, 19, 4)]
    [InlineData("smallmoney", MssTypes.SmallMoney, 4, 10, 4)]
    [InlineData("decimal(1)", MssTypes.Decimal, 5, 1, 0)]
    [InlineData("decimal(10,2)", MssTypes.Decimal, 9, 10, 2)]
    [InlineData("decimal(20)", MssTypes.Decimal, 13, 20, 0)]
    [InlineData("decimal(29)", MssTypes.Decimal, 17, 29, 0)]
    [InlineData("decimal(38,37)", MssTypes.Decimal, 17, 38, 37)]
    [InlineData("numeric(28,8)", MssTypes.Decimal, 13, 28, 8)]
    // SQL Server treats float(n) as one of two possible values.
    // If 1<=n<=24, n is treated as 24.
    // If 25<=n<=53, n is treated as 53.
    // https://learn.microsoft.com/en-us/sql/t-sql/data-types/float-and-real-transact-sql?view=sql-server-ver16
    [InlineData("float", MssTypes.Float, 8, 53, null)]
    [InlineData("float(10)", MssTypes.Real, 4, 24, null)]
    [InlineData("float(25)", MssTypes.Float, 8, 53, null)]
    [InlineData("real", MssTypes.Real, 4, 24, null)]
    [InlineData("date", MssTypes.Date, 3, 10, null)]
    [InlineData("datetime", MssTypes.DateTime, 8, null, null)]
    [InlineData("datetime2", MssTypes.DateTime2, 8, 27, 7)]
    [InlineData("datetime2(1)", MssTypes.DateTime2, 6, 21, 1)]
    [InlineData("datetime2(3)", MssTypes.DateTime2, 7, 23, 3)]
    [InlineData("datetime2(5)", MssTypes.DateTime2, 8, 25, 5)]
    [InlineData("smalldatetime", MssTypes.SmallDateTime, 4, null, null)]
    [InlineData("datetimeoffset", MssTypes.DateTimeOffset, 10, 34, 7)]
    [InlineData("datetimeoffset(0)", MssTypes.DateTimeOffset, 8, 26, 0)]
    [InlineData("datetimeoffset(1)", MssTypes.DateTimeOffset, 8, 28, 1)]
    [InlineData("datetimeoffset(2)", MssTypes.DateTimeOffset, 8, 29, 2)]
    [InlineData("datetimeoffset(3)", MssTypes.DateTimeOffset, 9, 30, 3)]
    [InlineData("datetimeoffset(4)", MssTypes.DateTimeOffset, 9, 31, 4)]
    [InlineData("datetimeoffset(5)", MssTypes.DateTimeOffset, 10, 32, 5)]
    [InlineData("datetimeoffset(6)", MssTypes.DateTimeOffset, 10, 33, 6)]
    [InlineData("datetimeoffset(7)", MssTypes.DateTimeOffset, 10, 34, 7)]
    [InlineData("time", MssTypes.Time, 5, 16, 7)]
    [InlineData("time(0)", MssTypes.Time, 3, 8, 0)]
    [InlineData("time(1)", MssTypes.Time, 3, 10, 1)]
    [InlineData("time(2)", MssTypes.Time, 3, 11, 2)]
    [InlineData("time(3)", MssTypes.Time, 4, 12, 3)]
    [InlineData("time(4)", MssTypes.Time, 4, 13, 4)]
    [InlineData("time(5)", MssTypes.Time, 5, 14, 5)]
    [InlineData("time(6)", MssTypes.Time, 5, 15, 6)]
    [InlineData("time(7)", MssTypes.Time, 5, 16, 7)]
    [InlineData("char(1)", MssTypes.Char, 1, null, null)]
    [InlineData("char(8000)", MssTypes.Char, 8000, null, null)]
    [InlineData("varchar(1)", MssTypes.VarChar, 1, null, null)]
    [InlineData("varchar(8000)", MssTypes.VarChar, 8000, null, null)]
    [InlineData("varchar(max)", MssTypes.VarChar, -1, null, null)]
    [InlineData("nchar(1)", MssTypes.NChar, 1, null, null)]
    [InlineData("nchar(4000)", MssTypes.NChar, 4000, null, null)]
    [InlineData("nvarchar(1)", MssTypes.NVarChar, 1, null, null)]
    [InlineData("nvarchar(4000)", MssTypes.NVarChar, 4000, null, null)]
    [InlineData("nvarchar(max)", MssTypes.NVarChar, -1, null, null)]
    [InlineData("ntext", MssTypes.NText, -1, null, null)]
    public async Task TestTypeAndSize(
        string colDef,
        string type,
        int length,
        int? precision,
        int? scale)
    {
        // Arrange
        await MssDbHelper.Instance.DropTable(_tableName);
        await MssDbHelper.Instance.ExecuteNonQuery($"create table {_tableName}(col1 {colDef})");
        var tableHeader = await MssSystemTables.GetTableHeaderAsync(_tableName, CancellationToken.None);
        tableHeader.Should().NotBeNull();

        // Act
        var columnInfo = (await MssSystemTables.GetTableColumnInfoAsync(tableHeader!, CancellationToken.None)).ToList();

        // Assert
        columnInfo.Should().NotBeNull();
        var col = columnInfo.First();
        col.Type.Should().Be(type, $"because column should be of type {type}");
        col.Length.Should().Be(length, $"because length of {type} should be {length}");
        col.Precision.Should().Be(precision, $"because precision of {type} should be {precision}");
        col.Scale.Should().Be(scale, $"because scale of {type} should be {scale}");
    }
}