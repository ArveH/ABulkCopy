namespace SqlServer.Tests;

[Collection(nameof(DatabaseCollection))]
public class MssReaderTests : MssTestBase
{
    private readonly ILogger _output;

    public MssReaderTests(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
        _output = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();
    }

    [Fact]
    public async Task TestDataReader_ReadBigint()
    {
        // Arrange
        var tableName = GetName();
        await DropTableAsync(tableName);
        await ExecuteNonQueryAsync($"create table {tableName} (col1 bigint)");
        await InsertIntoSingleColumnTableAsync(
            tableName, AllTypes.SampleValues.BigInt);

        var selectCreatorMock = new Mock<ISelectCreator>();
        selectCreatorMock
            .Setup(m => m.CreateSelect(It.IsAny<TableDefinition>()))
            .Returns($"select col1 from [{tableName}]");
        IMssTableReader tableReader = new MssTableReader(
            selectCreatorMock.Object,
            _output)
        {
            ConnectionString = DbFixture.MssConnectionString
        };

        var tableDefinition = MssTestData.GetTableDefinitionAllTypes();
        var cts = new CancellationTokenSource();

        // Act
        await tableReader.PrepareReaderAsync(tableDefinition, cts.Token);
        var rowWasRead = await tableReader.ReadAsync(cts.Token);
        rowWasRead.Should().BeTrue("because there should be one row");
        var value = tableReader.GetValue(0);
        value.Should().NotBeNull("because value should be a bigint number");
        value.Should().BeOfType<long>("because value should be a bigint number");
        ((long)value!).Should().Be(AllTypes.SampleValues.BigInt);
        rowWasRead = await tableReader.ReadAsync(cts.Token);
        rowWasRead.Should().BeFalse("because there should be only one row");
    }
}