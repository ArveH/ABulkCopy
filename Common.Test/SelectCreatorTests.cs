namespace Common.Test;

public class SelectCreatorTests
{
    private readonly ILogger _output;

    public SelectCreatorTests(ITestOutputHelper output)
    {
        _output = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();
    }

    [Fact]
    public void TestCreate_When_SingleColumn()
    {
        // Arrange
        var tableDefinition = new TableDefinition
        {
            Header = new TableHeader
            {
                Id = 1,
                Identity = new Identity(),
                Location = "default",
                Name = "mytable",
                Schema = "dbo"
            },
            Columns = new List<ColumnDefinition>
            {
                MssTestData.GetCharColDefinition(1, "col1", "nvarchar", 20)
            }
        };
        ISelectCreator creator = new SelectCreator(_output);

        // Act
        var selectStatement = creator.CreateSelect(tableDefinition).ToLowerInvariant();

        // Assert
        selectStatement.Should().Be("select col1 from dbo.mytable");
    }

    [Fact]
    public void TestCreate_When_3Columns()
    {
        // Arrange
        var tableDefinition = new TableDefinition
        {
            Header = new TableHeader
            {
                Id = 1,
                Identity = new Identity(),
                Location = "default",
                Name = "mytable",
                Schema = "dbo"
            },
            Columns = new List<ColumnDefinition>
            {
                MssTestData.GetCharColDefinition(1, "col1", "nvarchar", 20),
                MssTestData.GetCharColDefinition(2, "col2", "nvarchar", 20),
                MssTestData.GetCharColDefinition(3, "col3", "nvarchar", 20)
            }
        };
        ISelectCreator creator = new SelectCreator(_output);

        // Act
        var selectStatement = creator.CreateSelect(tableDefinition).ToLowerInvariant();

        // Assert
        selectStatement.Should().Be("select col1, col2, col3 from dbo.mytable");
    }
}