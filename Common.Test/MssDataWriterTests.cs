using System.Data;

namespace Common.Test;

public class MssDataWriterTests
{
    private const string TestPath = @"C:\testfiles";
    private readonly string _testTableName = Environment.MachineName + "MssDataWriterTests";
    private readonly ILogger _logger;
    private readonly TableDefinition _originalTableDefinition;
    private readonly MockFileSystem _mockFileSystem;
    private readonly DataWriter _dataWriter;

    public MssDataWriterTests(ITestOutputHelper output)
    {
        _logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        _originalTableDefinition = MssTestData.GetEmpty(_testTableName);
        _mockFileSystem = new MockFileSystem();
        _mockFileSystem.AddDirectory(TestPath);
        _dataWriter = new DataWriter(_mockFileSystem, _logger);
    }

    private async Task<string> ArrangeAndAct(IColumn col, object? value, SqlDbType? dbType = null)
    {
        // Arrange
        _originalTableDefinition.Columns.Add(col);
        await MssDbHelper.Instance.DropTable(_testTableName);
        await MssDbHelper.Instance.CreateTable(_originalTableDefinition);
        await MssDbHelper.Instance.InsertIntoSingleColumnTable(
            _testTableName, value, dbType);
        ITableReader tableReader = new MssTableReader(
            new SelectCreator(_logger), _logger)
        {
            ConnectionString = MssDbHelper.Instance.ConnectionString
        };

        // Act
        try
        {
            await _dataWriter.WriteTable(
                tableReader,
                _originalTableDefinition,
                TestPath);
        }
        finally
        {
            await MssDbHelper.Instance.DropTable(_testTableName);
        }

        return await GetJsonText();
    }

    private async Task TestWriteType(IColumn col, object? value)
    {
        // Assert
        var jsonTxt = await ArrangeAndAct(col, value);
        jsonTxt.TrimEnd().Should().Be(value + ",");
    }

    [Fact]
    public async Task TestWriteBigInt()
    {
        await TestWriteType(
            new SqlServerBigInt(101, "MyTestCol", false),
            AllTypes.SampleValues.BigInt);
    }

    [Fact]
    public async Task TestWriteInt()
    {
        await TestWriteType(
            new SqlServerInt(101, "MyTestCol", false),
            AllTypes.SampleValues.Int);
    }

    [Fact]
    public async Task TestWriteSmallInt()
    {
        await TestWriteType(
            new SqlServerSmallInt(101, "MyTestCol", false),
            AllTypes.SampleValues.SmallInt);
    }

    [Fact]
    public async Task TestWriteTinyInt()
    {
        await TestWriteType(
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
        await TestWriteType(
            new SqlServerMoney(101, "MyTestCol", false),
            AllTypes.SampleValues.Money);
    }

    [Fact]
    public async Task TestWriteDecimal()
    {
        await TestWriteType(
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
        await TestWriteType(
            new SqlServerFloat(101, "MyTestCol", false),
            AllTypes.SampleValues.Float);
    }

    [Fact]
    public async Task TestWriteReal()
    {
        await TestWriteType(
            new SqlServerReal(101, "MyTestCol", false),
            12.25);
    }

    [Fact]
    public async Task TestWriteDate()
    {
        var jsonTxt = await ArrangeAndAct(
            new SqlServerDate(101, "MyTestCol", false),
            new DateTime(2023, 5, 19));

        // Assert
        jsonTxt.TrimEnd().Should().Be("2023-05-19,");
    }

    [Fact]
    public async Task TestWriteDateTime_When_NoFraction()
    {
        var jsonTxt = await ArrangeAndAct(
            new SqlServerDateTime(101, "MyTestCol", false),
            new DateTime(2023, 5, 19, 11, 12, 13));

        // Assert
        jsonTxt.TrimEnd().Should().Be("2023-05-19T11:12:13.0000000,");
    }

    [Fact]
    public async Task TestWriteDateTime()
    {
        var jsonTxt = await ArrangeAndAct(
            new SqlServerDateTime(101, "MyTestCol", false),
            new DateTime(2023, 5, 19, 11, 12, 13, 233));

        // Assert
        jsonTxt.TrimEnd().Should().Be("2023-05-19T11:12:13.2330000,");
    }

    [Fact]
    public async Task TestWriteDateTime2()
    {
        var value = new DateTime(2023, 5, 19, 11, 12, 13, 55);

        var jsonTxt = await ArrangeAndAct(
            new SqlServerDateTime2(101, "MyTestCol", false),
            value,
            SqlDbType.DateTime2);

        // Assert
        jsonTxt.TrimEnd().Should().Be("2023-05-19T11:12:13.0550000,");
    }

    [Fact]
    public async Task TestWriteDateTime2_When_NanoSeconds()
    {
        var value = new DateTime(2023, 5, 19, 11, 12, 13, 233, 666).AddTicks(8);

        var jsonTxt = await ArrangeAndAct(
            new SqlServerDateTime2(101, "MyTestCol", false),
            value,
            SqlDbType.DateTime2);

        // Assert
        jsonTxt.TrimEnd().Should().Be("2023-05-19T11:12:13.2336668,");
    }

    [Fact]
    public async Task TestWriteDateTimeOffset()
    {
        var value = new DateTimeOffset(2023, 5, 19, 11, 12, 13, 233, 666, new TimeSpan(1, 0, 0)).AddTicks(8);

        var jsonTxt = await ArrangeAndAct(
            new SqlServerDatetimeOffset(101, "MyTestCol", false),
            value,
            SqlDbType.DateTimeOffset);

        // Assert
        jsonTxt.TrimEnd().Should().Be("2023-05-19T11:12:13.2336668+01:00,");
    }

    [Fact]
    public async Task TestWriteTime()
    {
        var value = new TimeSpan(11, 12, 13);

        var jsonTxt = await ArrangeAndAct(
            new SqlServerTime(101, "MyTestCol", false),
            value,
            SqlDbType.Time);

        // Assert
        jsonTxt.TrimEnd().Should().Be("11:12:13,");
    }

    private async Task TestWriteString(
        IColumn col,
        string actual,
        string expected)
    {
        var jsonTxt = await ArrangeAndAct(col, actual);

        // Assert
        jsonTxt.TrimEnd().Should().Be($"{expected},");
    }

    [Fact]
    public async Task TestWriteChar()
    {
        await TestWriteString(
            new SqlServerChar(101, "MyTestCol", false, 10),
                "Arve",
                "'Arve      '");
    }

    [Fact]
    public async Task TestWriteChar_When_SingleQuote_Then_TwoQuotesInFile()
    {
        await TestWriteString(
            new SqlServerChar(101, "MyTestCol", false, 15),
            "Arve's vodka",
            "'Arve''s vodka   '");
    }

    [Fact]
    public async Task TestWriteChar_When_BackSlashLastCharacter()
    {
        await TestWriteString(
            new SqlServerChar(101, "MyTestCol", false, 10),
            "123456789\\",
            "'123456789\\'");
    }

    [Fact]
    public async Task TestWriteVarChar_When_SingleQuote_Then_TwoQuotesInFile()
    {
        await TestWriteString(
            new SqlServerVarChar(101, "MyTestCol", false, 15),
            "Arve's vodka",
            "'Arve''s vodka'");
    }

    [Fact]
    public async Task TestWriteVarChar_When_BackSlash()
    {
        await TestWriteString(
            new SqlServerVarChar(101, "MyTestCol", false, 10),
            "\\Arve",
            "'\\Arve'");
    }

    private static readonly string String10K = new ('a', 10000);
    [Fact]
    public async Task TestWriteVarCharMax()
    {
        await TestWriteString(
            new SqlServerVarChar(101, "MyTestCol", false, -1),
            String10K,
            "'" + String10K + "'");
    }

    [Fact]
    public async Task TestWriteNChar()
    {
        await TestWriteString(
            new SqlServerNChar(101, "MyTestCol", false, 10),
            "Arveﯵ",
            "'Arveﯵ'");
    }

    [Fact]
    public async Task TestWriteNChar_When_SingleQuote_Then_TwoQuotesInFile()
    {
        await TestWriteString(
            new SqlServerNChar(101, "MyTestCol", false, 15),
            "Arveﯵ's vodka",
            "'Arveﯵ''s vodka'");
    }

    [Fact]
    public async Task TestWriteNChar_When_BackSlashLastCharacter()
    {
        await TestWriteString(
            new SqlServerNChar(101, "MyTestCol", false, 10),
            "12345678ﯵ\\",
            "'12345678ﯵ\\'");
    }

    [Fact]
    public async Task TestWriteNVarChar_When_SingleQuote_Then_TwoQuotesInFile()
    {
        await TestWriteString(
            new SqlServerNVarChar(101, "MyTestCol", false, 15),
            "Arveﯵ's vodka",
            "'Arveﯵ''s vodka'");
    }

    [Fact]
    public async Task TestWriteNVarChar_When_BackSlash()
    {
        await TestWriteString(
            new SqlServerNVarChar(101, "MyTestCol", false, 10),
            "\\ﯵArve",
            "'\\ﯵArve'");
    }

    private static readonly string NString10K = new ('ﯵ', 10000);
    [Fact]
    public async Task TestWriteNVarCharMax()
    {
        await TestWriteString(
            new SqlServerNVarChar(101, "MyTestCol", false, -1),
            NString10K,
            "'" + NString10K + "'");
    }

    [Fact]
    public async Task TestWriteGuid()
    {
        var value = Guid.NewGuid();

        var jsonTxt = await ArrangeAndAct(
            new SqlServerUniqueIdentifier(101, "MyTestCol", false),
            value);

        // Assert
        jsonTxt.TrimEnd().Should().Be(value + ",");
    }

    private async Task<string> GetJsonText()
    {
        var fullPath = Path.Combine(TestPath, _testTableName + CommonConstants.DataSuffix);
        _mockFileSystem.FileExists(fullPath).Should().BeTrue("because data file should exist");
        var jsonTxt = await _mockFileSystem.File.ReadAllTextAsync(fullPath);
        return jsonTxt;
    }
}