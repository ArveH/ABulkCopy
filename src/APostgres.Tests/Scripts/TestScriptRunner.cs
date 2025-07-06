namespace APostgres.Tests.Scripts;

public class TestScriptRunner
{
    [Fact]
    public async Task TestRunner_When_FileNotExists()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem();
        var scriptReader = new ScriptReader(mockFileSystem);
        var mockRawCommand = new Mock<IDbRawCommand>();
        var mockLogger = new Mock<ILogger>();

        // Act
        var runner = new ScriptRunner(
            scriptReader, mockRawCommand.Object, mockLogger.Object);

        // Assert
        await Assert.ThrowsAsync<FileNotFoundException>(
            () => runner.ExecuteAsync("NonExistentScript.sql", CancellationToken.None));
    }

    [Fact]
    public async Task TestRunner_When_TwoStatements()
    {
        // Arrange
        var mockScriptReader = new Mock<IScriptReader>();
        var mockLogger = new Mock<ILogger>();
        mockScriptReader
            .Setup(r => r.ReadAsync(It.IsAny<string>(), CancellationToken.None))
            .Returns(GetMockedItems());
        var mockRawCommand = new Mock<IDbRawCommand>();
        var runner = new ScriptRunner(
            mockScriptReader.Object, mockRawCommand.Object, mockLogger.Object);
        
        // Act
        await runner.ExecuteAsync("script.sql", CancellationToken.None);
        
        // Assert
        mockRawCommand.Verify(
            r => r.ExecuteNonQueryAsync(It.IsAny<string>(), CancellationToken.None), 
            Times.Exactly(2));
    }
    
    static async IAsyncEnumerable<string> GetMockedItems()
    {
        yield return "First";
        yield return "Second";
        await Task.CompletedTask; // optional, makes it async
    }
}