namespace Common.Test;

public class SchemaWriterMssTests
{
    private const string TestPath = @"C:\testfiles";
    private const string TestTableName = "TestTableForTestWrite";
    private readonly TableDefinition _originalTableDefinition;
    private readonly MockFileSystem _mockFileSystem;
    private readonly ISchemaWriter _schemaWriter;

    public SchemaWriterMssTests()
    {
        _originalTableDefinition = MssTestData.GetEmpty(TestTableName);
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
        var col = new SqlServerDatetime2(101, "MyTestCol", false);
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
    public async Task TestWriteFloat_When_Scale14()
    {
        var col = new SqlServerFloat(101, "MyTestCol", false, 14);
        _originalTableDefinition.Columns.Add(col);
        var expectedCol = col.Clone();
        expectedCol.Type = ColumnType.SmallFloat;
        expectedCol.Precision = 24;
        await TestWriteColumn_When_NoChange(expectedCol);
    }

    private async Task TestWriteColumn_When_NoChange(IColumn col)
    {
        _originalTableDefinition.Columns.Add(col);
        await TestWriteColumn(col);
    }

    private async Task TestWriteColumn(IColumn col)
    {
        // Act
        await _schemaWriter.Write(_originalTableDefinition, TestPath);

        // Assert
        var jsonTxt = await GetJsonText();
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

    private async Task<string> GetJsonText()
    {
        var fullPath = Path.Combine(TestPath, TestTableName + CommonConstants.SchemaSuffix);
        _mockFileSystem.FileExists(fullPath).Should().BeTrue("because schema file should exist");
        var jsonTxt = await _mockFileSystem.File.ReadAllTextAsync(fullPath);
        return jsonTxt;
    }
}