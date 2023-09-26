namespace Common.Test;

public class IdentifierTest : CommonTestBase
{
    public IdentifierTest(ITestOutputHelper output) 
        : base(output)
    {
    }

    [Fact]
    public void TestAdjustForSystemTable_When_AddQuoteIsFalse_And_NameIsKeyword_Then_NotChanged()
    {
        // Arrange
        var appSettings = new Dictionary<string, string?>
        {
            {"AddQuotes", "true"},
            {"QuoteIdentifiers:0", "select"}
        };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettings)
            .Build();
        var dbContextMock = new Mock<IDbContext>();
        dbContextMock.Setup(x => x.Rdbms).Returns(Rdbms.Pg);
        var identifier = new Identifier(config, dbContextMock.Object);

        // Act
        var result = identifier.AdjustForSystemTable("Select");

        // Assert
        result.Should().Be("Select");
    }

}