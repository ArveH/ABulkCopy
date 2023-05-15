namespace Common.Test;

public class ASchemaWriterTests
{
    [Fact]
    public async Task TestWrite_When_AllTypes()
    {
        // Arrange
        var path = @"C:\backup";
        var mockFIleSystem = new MockFileSystem();
        mockFIleSystem.AddDirectory(path);
        IASchemaWriter schemaWriter = new ASchemaWriter(
            mockFIleSystem,
            new LoggerConfiguration().CreateLogger());
        var fullPath = Path.Combine(path, "AllTypes.schema");
        
        // Act
        await schemaWriter.Write(MssTestData.GetTableDefinitionAllTypes(), fullPath);

        // Assert
        mockFIleSystem.FileExists(fullPath).Should().BeTrue();
    }
}