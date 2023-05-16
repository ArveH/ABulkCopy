namespace ASqlServer.Test;

public class MssReaderTests
{
    private readonly ILogger _output;
    private readonly IConfiguration _configuration;

    public MssReaderTests(ITestOutputHelper output)
    {
        _output = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        _configuration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");
    }

    [Fact]
    public async Task TestDataReader_ReadBigint()
    {
        // Arrange
        var connectionString = _configuration.GetConnectionString("FromDb");
        connectionString.Should()
            .NotBeNullOrWhiteSpace("because the connection string should be set in the user secrets file");
        var selectCreatorMock = new Mock<ISelectCreator>();
        selectCreatorMock
            .Setup(m => m.CreateSelect(It.IsAny<TableDefinition>()))
            .Returns("select ExactNumBigInt from dbo.AllTypes");
        IMssDataReader dataReader = new MssDataReader(
            selectCreatorMock.Object,
            _output)
        {
            ConnectionString = connectionString!
        };

        var tableDefinition = MssTestData.GetTableDefinitionAllTypes();

        // Act
        await dataReader.PrepareReader(tableDefinition);
        while (await dataReader.Read())
        {

        }

    }
}