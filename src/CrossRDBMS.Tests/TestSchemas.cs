namespace CrossRDBMS.Tests;

[Collection(nameof(DatabaseCollection))]
public class TestSchemas : TestBase
{
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _output;

    public TestSchemas(DatabaseFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }

    [Fact]
    public async Task TestFKInAnotherSchema()
    {
        await Task.CompletedTask;
    }
}