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

    [Fact]
    public async Task TestRaw()
    {
        var value = AllTypes.SampleValues.Varbinary10K;
        var col = new SqlServerVarBinary(101, "MyTestCol", false, 10000);
        OriginalTableDefinition.Columns.Add(col);
        await MssDbHelper.Instance.DropTable(TestTableName);
        await MssDbHelper.Instance.CreateTable(OriginalTableDefinition);
        await MssDbHelper.Instance.InsertIntoSingleColumnTable(
            TestTableName, value, SqlDbType.VarBinary);

        // Act
        try
        {
            await TestDataWriter.Write(
                OriginalTableDefinition,
                TestPath);
        }
        finally
        {
            await MssDbHelper.Instance.DropTable(TestTableName);
        }

    }
}