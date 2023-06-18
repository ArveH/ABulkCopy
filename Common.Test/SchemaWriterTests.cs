namespace Common.Test;

public class SchemaWriterTests
{
    private const string TestPath = @"C:\testfiles";
    private const string TestTableName = "TestTableForTestWrite";
    private readonly TableDefinition _originalTableDefinition;
    private readonly MockFileSystem _mockFileSystem;
    private readonly ISchemaWriter _schemaWriter;

    public SchemaWriterTests()
    {
        _originalTableDefinition = MssTestData.GetEmpty(TestTableName);
        _mockFileSystem = new MockFileSystem();
        _mockFileSystem.AddDirectory(TestPath);
        _schemaWriter = new SchemaWriter(
            _mockFileSystem,
            new LoggerConfiguration().CreateLogger());
    }

    [Fact]
    public async Task TestWrite_When_NoIdentity()
    {
        // Arrange
        _originalTableDefinition.Columns.Add(new SqlServerBigInt(101, "Id", false));
        
        // Act
        await _schemaWriter.Write(_originalTableDefinition, TestPath);

        // Assert
        var jsonTxt = await GetJsonText();
        jsonTxt.Squeeze().Should().ContainEquivalentOf((
            "\"Header\": {\r\n" +
            "    \"Id\": 1,\r\n" +
            "    \"Name\": \"TestTableForTestWrite\",\r\n" +
            "    \"Schema\": \"dbo\",\r\n" +
            "    \"Location\": \"default\",\r\n" +
            "    \"Identity\": null\r\n" +
            "}").Squeeze());
    }

    [Fact]
    public async Task TestWrite_When_Identity()
    {
        // Arrange
        _originalTableDefinition.Header.Identity = new Identity();
        _originalTableDefinition.Columns.Add(new SqlServerBigInt(101, "Id", false)
        {
            Identity = _originalTableDefinition.Header.Identity
        });
        
        // Act
        await _schemaWriter.Write(_originalTableDefinition, TestPath);

        // Assert
        var jsonTxt = await GetJsonText();
        jsonTxt.Squeeze().Should().ContainEquivalentOf((
            "\"Header\": {\r\n" +
            "    \"Id\": 1,\r\n" +
            "    \"Name\": \"TestTableForTestWrite\",\r\n" +
            "    \"Schema\": \"dbo\",\r\n" +
            "    \"Location\": \"default\",\r\n" +
            "    \"Identity\": {\r\n" +
            "        \"Seed\": 1,\r\n" +
            "        \"Increment\": 1\r\n" +
            "      }\r\n" +
            "}").Squeeze());
    }

    private async Task<string> CreateJsonSchema(DefaultDefinition def)
    {
        // Arrange
        _originalTableDefinition.Columns = new List<IColumn>
        {
            new SqlServerInt(101, "intdef", false)
            {
                DefaultConstraint = def
            }
        };
        
        // Act
        await _schemaWriter.Write(_originalTableDefinition, TestPath);

        // Assert
        return await GetJsonText();
    }

    [Fact]
    public async Task TestWriteDefault_When_IntAndNamed()
    {
        var jsonTxt = await CreateJsonSchema(
            new DefaultDefinition
            {
                Name = "df_bulkcopy_int",
                Definition = "((0))",
                IsSystemNamed = false
            });

        jsonTxt.Squeeze().Should().ContainEquivalentOf((
            "\"DefaultConstraint\": {\r\n" +
            "   \"Name\": \"df_bulkcopy_int\",\r\n" +
            "   \"Definition\": \"((0))\",\r\n" +
            "   \"IsSystemNamed\": false\r\n" +
            " }").Squeeze());
    }

    [Fact]
    public async Task TestWriteDefault_When_StrAndAutoNamed()
    {
        var jsonTxt = await CreateJsonSchema(
            new DefaultDefinition
            {
                Name = "DF__arveh__col1__531856C7",
                Definition = "('Norway')",
                IsSystemNamed = true
            });

        // TODO: Fix the unicoded quotes 
        jsonTxt.Squeeze().Should().ContainEquivalentOf((
            "\"DefaultConstraint\": {\r\n" +
            "   \"Name\": \"DF__arveh__col1__531856C7\",\r\n" +
            "   \"Definition\": \"('Norway')\",\r\n" +
            "   \"IsSystemNamed\": true\r\n" +
            " }").Squeeze());
    }

    [Fact]
    public async Task TestWriteDefault_When_DateAndAutoNamed()
    {
        var jsonTxt = await CreateJsonSchema(
            new DefaultDefinition
            {
                Name = "DF__arveh__col1__531856C8",
                Definition = "(getdate())",
                IsSystemNamed = true
            });

        jsonTxt.Squeeze().Should().ContainEquivalentOf((
            "\"DefaultConstraint\": {\r\n" +
            "   \"Name\": \"DF__arveh__col1__531856C8\",\r\n" +
            "   \"Definition\": \"(getdate())\",\r\n" +
            "   \"IsSystemNamed\": true\r\n" +
            " }").Squeeze());
    }

    private async Task<string> GetJsonText()
    {
        var fullPath = Path.Combine(TestPath, TestTableName + Rdbms.Mss.SchemaSuffix());
        _mockFileSystem.FileExists(fullPath).Should().BeTrue("because schema file should exist");
        var jsonTxt = await _mockFileSystem.File.ReadAllTextAsync(fullPath);
        return jsonTxt;
    }
}