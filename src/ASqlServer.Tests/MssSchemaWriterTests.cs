using ABulkCopy.Common.Extensions;

namespace ASqlServer.Tests;

public class MssSchemaWriterTests
{
    private const string TestPath = @"C:\testfiles";
    private static SchemaTableTuple TestNames = ("dbo", "TestTableForTestWrite");
    private readonly TableDefinition _originalTableDefinition;
    private readonly MockFileSystem _mockFileSystem;
    private readonly ISchemaWriter _schemaWriter;

    public MssSchemaWriterTests()
    {
        _originalTableDefinition = MssTestData.GetEmpty(TestNames);
        _mockFileSystem = new MockFileSystem();
        _mockFileSystem.AddDirectory(TestPath);
        _schemaWriter = new SchemaWriter(
            _mockFileSystem,
            new LoggerConfiguration().CreateLogger());
    }

    [Fact]
    public async Task TestWriteBigInt()
    {
        var col = new SqlServerBigInt(101, "MyTestCol", false);
        await TestWriteColumn_When_NoChange(col);
    }

    [Fact]
    public async Task TestWriteDecimal()
    {
        var col = new SqlServerDecimal(101, "MyTestCol", false, 20, 5);
        await TestWriteColumn_When_NoChange(col);
    }

    [Fact]
    public async Task TestWriteDateTime()
    {
        var col = new SqlServerDateTime2(101, "MyTestCol", false);
        await TestWriteColumn_When_NoChange(col);
    }

    [Fact]
    public async Task TestWriteNVarChar()
    {
        var col = new SqlServerNVarChar(101, "MyTestCol", false, 50, "SQL_Latin1_General_CP1_CI_AS");
        await TestWriteColumn_When_NoChange(col);
    }

    [Fact]
    public async Task TestWriteUniqueIdentifier()
    {
        var col = new SqlServerUniqueIdentifier(101, "MyTestCol", false);
        await TestWriteColumn_When_NoChange(col);
    }

    [Fact]
    public async Task TestWriteBit()
    {
        var col = new SqlServerBit(101, "MyTestCol", false);
        await TestWriteColumn_When_NoChange(col);
    }

    [Fact]
    public async Task TestWriteFloat_When_Scale14_Then_SmallFloatAnd24()
    {
        var col = new SqlServerFloat(101, "MyTestCol", false, 14);
        _originalTableDefinition.Columns.Add(col);
        var expectedCol = col.Clone();
        expectedCol.Type = MssTypes.Real;
        expectedCol.Precision = 24;
        await TestWriteColumn(expectedCol);
    }

    [Fact]
    public async Task TestWriteFloat_When_Scale25_Then_Scale53()
    {
        var col = new SqlServerFloat(101, "MyTestCol", false, 25);
        _originalTableDefinition.Columns.Add(col);
        var expectedCol = col.Clone();
        expectedCol.Type = MssTypes.Float;
        expectedCol.Precision = 53;
        await TestWriteColumn(expectedCol);
    }

    [Fact]
    public async Task TestWriteNVarChar_When_Length4000_Then_Unchanged()
    {
        var col = new SqlServerNVarChar(101, "MyTestCol", false, 4000);
        _originalTableDefinition.Columns.Add(col);
        var expectedCol = col.Clone();
        expectedCol.Type = MssTypes.NVarChar;
        expectedCol.Length = 4000;
        await TestWriteColumn(expectedCol);
    }

    [Fact]
    public async Task TestWriteNVarChar_When_Length4001_Then_NLongText()
    {
        var col = new SqlServerNVarChar(101, "MyTestCol", false, 4001);
        _originalTableDefinition.Columns.Add(col);
        var expectedCol = col.Clone();
        expectedCol.Type = MssTypes.NVarChar;
        expectedCol.Length = -1;
        await TestWriteColumn(expectedCol);
    }

    [Fact]
    public async Task TestWriteVarBinary_When_Length8000_Then_Unchanged()
    {
        var col = new SqlServerVarBinary(101, "MyTestCol", false, 8000);
        _originalTableDefinition.Columns.Add(col);
        var expectedCol = col.Clone();
        expectedCol.Type = MssTypes.VarBinary;
        expectedCol.Length = 8000;
        await TestWriteColumn(expectedCol);
    }

    [Fact]
    public async Task TestWriteVarBinary_When_Length8001_Then_MaxLength()
    {
        var col = new SqlServerVarBinary(101, "MyTestCol", false, 8001);
        _originalTableDefinition.Columns.Add(col);
        var expectedCol = col.Clone();
        expectedCol.Type = MssTypes.VarBinary;
        expectedCol.Length = -1;
        await TestWriteColumn(expectedCol);
    }
    
    private async Task TestWriteColumn_When_NoChange(IColumn col)
    {
        _originalTableDefinition.Columns.Add(col);
        await TestWriteColumn(col);
    }

    private async Task TestWriteColumn(IColumn col)
    {
        // Act
        await _schemaWriter.WriteAsync(_originalTableDefinition, TestPath);

        // Assert
        var jsonTxt = await _mockFileSystem.GetJsonSchemaText(TestPath, TestNames);
        var collation = col.Collation == null ? "null" : $"\"{col.Collation}\"";
        jsonTxt.Squeeze().Should().ContainEquivalentOf((
            "{\r\n" +
            $"      \"Name\": \"{col.Name}\",\r\n" +
            $"      \"Type\": \"{col.Type}\",\r\n" +
            $"      \"IsNullable\": {col.IsNullable},\r\n" +
            "      \"Identity\": null,\r\n" +
            "      \"ComputedDefinition\": null,\r\n" +
            $"      \"Length\": {col.Length},\r\n" +
            $"      \"Precision\": {col.Precision?.ToString()??"null"},\r\n" +
            $"      \"Scale\": {col.Scale?.ToString()??"null"},\r\n" +
            "      \"DefaultConstraint\": null,\r\n" +
            $"      \"Collation\": {collation}\r\n" +
            "    }").Squeeze());
    }
}