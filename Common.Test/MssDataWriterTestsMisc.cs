namespace Common.Test;

public class MssDataWriterTestsMisc : MssDataWriterTestBase
{
    public override string _testTableName => Environment.MachineName + "MssDataWriterTestsMisc";

    public MssDataWriterTestsMisc(ITestOutputHelper output)
        : base(output)
    {
    }

    [Fact]
    public async Task TestWriteGuid()
    {
        var value = Guid.NewGuid();

        var jsonTxt = await ArrangeAndAct(
            new SqlServerUniqueIdentifier(101, "MyTestCol", false),
            value);

        // Assert
        jsonTxt.TrimEnd().Should().Be(value + ",");
    }
}