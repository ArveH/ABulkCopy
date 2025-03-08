namespace SqlServer.Tests.SqlScript;

[Collection(nameof(DatabaseCollection))]
public class MssScriptTests : MssTestBase
{
    public MssScriptTests(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }
}