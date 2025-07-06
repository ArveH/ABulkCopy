namespace Postgres.Tests.Scripts;

[Collection(nameof(DatabaseCollection))]
public class TestScriptRunner(DatabaseFixture dbFixture, ITestOutputHelper output) 
    : PgTestBase(dbFixture, output)
{
    [Fact]
    public async Task TestRunScript_When_LegalSql()
    {
        // Arrange
        const string fileName = "test_script_runner.sql";
        var mockFileSystem = new MockFileSystem();
        var fileData = new MockFileData("create table if not exists mytable(int col1)");
        mockFileSystem.AddFile(Path.Combine(".", fileName), fileData);
        var scriptReader = new ScriptReader(mockFileSystem);
        var runner = new ScriptRunner(
            scriptReader, dbFixture.PgRawCommand, TestLogger);

        // Act
        var (succeedCounter, errorCounter) = await runner.ExecuteAsync(fileName, CancellationToken.None);
        
        // Assert
        succeedCounter.Should().Be(0);
        errorCounter.Should().Be(1);
    }
    
    [Fact]
    public async Task TestRunScript_When_IllegalSql()
    {
        // Arrange
        const string fileName = "test_script_runner.sql";
        var mockFileSystem = new MockFileSystem();
        var fileData = new MockFileData("this is not sql");
        mockFileSystem.AddFile(Path.Combine(".", fileName), fileData);
        var scriptReader = new ScriptReader(mockFileSystem);
        var runner = new ScriptRunner(
            scriptReader, dbFixture.PgRawCommand, TestLogger);

        // Act
        var (succeedCounter, errorCounter) = await runner.ExecuteAsync(fileName, CancellationToken.None);
        
        // Assert
        succeedCounter.Should().Be(0);
        errorCounter.Should().Be(1);
    }
}