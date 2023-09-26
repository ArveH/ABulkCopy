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
            {"AddQuotes", "false"},
            {"QuoteIdentifiers:0", "select"}
        };
        var identifier = GetIdentifier(appSettings);

        // Act
        var result = identifier.AdjustForSystemTable("Select");

        // Assert
        result.Should().Be("Select");
    }

    [Fact]
    public void TestAdjustForSystemTable_When_AddQuoteIsFalse_Then_LowerCase()
    {
        // Arrange
        var appSettings = new Dictionary<string, string?>
        {
            {"AddQuotes", "false"}
        };
        var identifier = GetIdentifier(appSettings);

        // Act
        var result = identifier.AdjustForSystemTable("Select");

        // Assert
        result.Should().Be("select");
    }

    [Fact]
    public void TestAdjustForSystemTable_When_AddQuoteIsTrue_Then_NotChanged()
    {
        // Arrange
        var appSettings = new Dictionary<string, string?>
        {
            {"AddQuotes", "true"}
        };
        var identifier = GetIdentifier(appSettings);

        // Act
        var result = identifier.AdjustForSystemTable("Select");

        // Assert
        result.Should().Be("Select");
    }

    [Fact]
    public void TestGet_When_AddQuoteIsTrue_Then_Quoted()
    {
        // Arrange
        var appSettings = new Dictionary<string, string?>
        {
            {"AddQuotes", "true"}
        };
        var identifier = GetIdentifier(appSettings);

        // Act
        var result = identifier.Get("Select");

        // Assert
        result.Should().Be("\"Select\"");
    }

    [Fact]
    public void TestGet_When_AddQuoteIsFalse_Then_NotQuoted()
    {
        // Arrange
        var appSettings = new Dictionary<string, string?>
        {
            {"AddQuotes", "false"}
        };
        var identifier = GetIdentifier(appSettings);

        // Act
        var result = identifier.Get("Select");

        // Assert
        result.Should().Be("Select");
    }

    [Fact]
    public void TestGet_When_AddQuoteIsFalse_And_NameIsKeyword_Then_Quoted()
    {
        // Arrange
        var appSettings = new Dictionary<string, string?>
        {
            {"AddQuotes", "false"},
            {"QuoteIdentifiers:0", "select"}
        };
        var identifier = GetIdentifier(appSettings);

        // Act
        var result = identifier.Get("Select");

        // Assert
        result.Should().Be("\"Select\"");
    }

    private static Identifier GetIdentifier(
        Dictionary<string, string?> appSettings,
        Rdbms rdbms = Rdbms.Pg)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettings)
            .Build();
        var dbContextMock = new Mock<IDbContext>();
        dbContextMock.Setup(x => x.Rdbms).Returns(rdbms);
        var identifier = new Identifier(config, dbContextMock.Object);
        return identifier;
    }
}