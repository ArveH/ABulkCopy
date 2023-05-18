namespace Common.Test;

public class ASchemaWriterTests
{
    private const string TestPath = @"C:\testfiles";
    private const string TestTableName = "TestTableForTestWrite";
    private readonly TableDefinition _originalTableDefinition;
    private readonly MockFileSystem _mockFileSystem;
    private readonly ISchemaWriter _schemaWriter;

    public ASchemaWriterTests()
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

    [Fact]
    public async Task TestWrite_When_DefaultValues()
    {
        // Arrange
        _originalTableDefinition.Columns = new List<IColumn>
        {
            MssTestData.GetIdColDefinition(101, "Id"),
            new SqlServerInt(101, "intdef", false)
            {
                DefaultConstraint = new DefaultDefinition
                {
                    Name = "df_bulkcopy_int",
                    Definition = "((0))",
                    IsSystemNamed = false
                }
            },
            new SqlServerNVarChar(102, "strdef", true, 20, "SQL_Latin1_General_CP1_CI_AS")
            {
                DefaultConstraint = new DefaultDefinition
                {
                    Name = "DF__arveh__col1__531856C7",
                    Definition = "('Norway')",
                    IsSystemNamed = true
                }
            },
            new SqlServerDatetime2(103, "datedef", true)
            {
                DefaultConstraint = new DefaultDefinition
                {
                    Name = "DF__arveh__col1__531856C8",
                    Definition = "(getdate())",
                    IsSystemNamed = true
                }
            }
        };
        
        // Act
        await _schemaWriter.Write(_originalTableDefinition, TestPath);

        // Assert
        var fullPath = Path.Combine(TestPath, TestTableName + CommonConstants.SchemaSuffix);
        _mockFileSystem.FileExists(fullPath).Should().BeTrue("because schema file should exist");
        var jsonTxt = await _mockFileSystem.File.ReadAllTextAsync(fullPath);
        var tableDefinition = JsonSerializer.Deserialize<TableDefinition>(jsonTxt);
        tableDefinition.Should().NotBeNull("because we should be able to deserialize the schema file");
        tableDefinition.Should().BeEquivalentTo(_originalTableDefinition);
        tableDefinition!.Header.Name.Should().Be(TestTableName);
    }

    private async Task<string> GetJsonText()
    {
        var fullPath = Path.Combine(TestPath, TestTableName + CommonConstants.SchemaSuffix);
        _mockFileSystem.FileExists(fullPath).Should().BeTrue("because schema file should exist");
        var jsonTxt = await _mockFileSystem.File.ReadAllTextAsync(fullPath);
        return jsonTxt;
    }
}