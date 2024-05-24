namespace CrossRDBMS.Tests;

[Collection(nameof(DatabaseCollection))]
public class TestSchemas : TestBase
{
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _output;
    private IIdentifier? _identifier;

    public TestSchemas(DatabaseFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }

    [Fact]
    public async Task TestFkInAnotherSchema()
    {
    }
}