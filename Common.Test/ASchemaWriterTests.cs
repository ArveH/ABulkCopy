namespace Common.Test;

public class ASchemaWriterTests
{
    [Fact]
    public async Task TestWrite_When_NoIdentity()
    {
        // Arrange
        var path = @"C:\backup";
        var tableName = "TestTableForTestWrite";
        var originalTableDefinition = MssTestData.GetEmpty(tableName);
        originalTableDefinition.Columns.Add(new SqlServerBigInt(101, "Id", false));
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddDirectory(path);
        ISchemaWriter schemaWriter = new SchemaWriter(
            mockFileSystem,
            new LoggerConfiguration().CreateLogger());
        
        // Act
        await schemaWriter.Write(originalTableDefinition, path);

        // Assert
        var fullPath = Path.Combine(path, tableName + CommonConstants.SchemaSuffix);
        mockFileSystem.FileExists(fullPath).Should().BeTrue("because schema file should exist");
        var jsonTxt = await mockFileSystem.File.ReadAllTextAsync(fullPath);
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
        var path = @"C:\backup";
        var tableName = "TestTableForTestWrite";
        var originalTableDefinition = MssTestData.GetEmpty(tableName);
        originalTableDefinition.Header.Identity = new Identity();
        originalTableDefinition.Columns.Add(new SqlServerBigInt(101, "Id", false)
        {
            Identity = originalTableDefinition.Header.Identity
        });
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddDirectory(path);
        ISchemaWriter schemaWriter = new SchemaWriter(
            mockFileSystem,
            new LoggerConfiguration().CreateLogger());
        
        // Act
        await schemaWriter.Write(originalTableDefinition, path);

        // Assert
        var fullPath = Path.Combine(path, tableName + CommonConstants.SchemaSuffix);
        mockFileSystem.FileExists(fullPath).Should().BeTrue("because schema file should exist");
        var jsonTxt = await mockFileSystem.File.ReadAllTextAsync(fullPath);
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
        var path = @"C:\backup";
        var originalTableDefinition = MssTestData.GetTableDefaults();
        var tableName = originalTableDefinition.Header.Name;
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddDirectory(path);
        ISchemaWriter schemaWriter = new SchemaWriter(
            mockFileSystem,
            new LoggerConfiguration().CreateLogger());
        
        // Act
        await schemaWriter.Write(originalTableDefinition, path);

        // Assert
        var fullPath = Path.Combine(path, tableName + CommonConstants.SchemaSuffix);
        mockFileSystem.FileExists(fullPath).Should().BeTrue("because schema file should exist");
        var jsonTxt = await mockFileSystem.File.ReadAllTextAsync(fullPath);
        var tableDefinition = JsonSerializer.Deserialize<TableDefinition>(jsonTxt);
        tableDefinition.Should().NotBeNull("because we should be able to deserialize the schema file");
        tableDefinition.Should().BeEquivalentTo(originalTableDefinition);
        tableDefinition!.Header.Name.Should().Be(tableName);
    }
}