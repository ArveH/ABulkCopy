using ABulkCopy.Common.Writer;

namespace Common.Test;

public class ASchemaWriterTests
{
    [Fact]
    public async Task TestWrite_When_AllTypes()
    {
        // Arrange
        var path = @"C:\backup";
        var originalTableDefinition = MssTestData.GetTableDefinitionAllTypes();
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