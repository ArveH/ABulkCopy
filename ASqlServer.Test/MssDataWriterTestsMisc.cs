namespace ASqlServer.Test;

public class MssDataWriterTestsMisc : MssDataWriterTestBase
{
    public MssDataWriterTestsMisc(ITestOutputHelper output)
        : base(output, Environment.MachineName + "MssDataWriterTestsMisc")
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