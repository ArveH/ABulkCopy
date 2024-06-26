﻿namespace Common.Tests;

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
        var selectStatement = TestCreate(1);

        // Assert
        selectStatement.Should().Be("select [col1] from [dbo].[mytable]");
    }

    [Fact]
    public void TestCreate_When_3Columns()
    {
        var selectStatement = TestCreate(3);

        // Assert
        selectStatement.Should().Be("select [col1], [col2], [col3] from [dbo].[mytable]");
    }

    [Fact]
    public void TestCreate_When_NoColumns_Then_Exception()
    {
        var act = () => TestCreate(0);

        act.Should().Throw<ArgumentException>()
            .WithMessage("No columns in table definition*");
    }

    private string TestCreate(int colCount)
    {
        var tableDefinition = new TableDefinition(Rdbms.Mss)
        {
            Header = new TableHeader
            {
                Id = 1,
                Identity = new Identity(),
                Location = "default",
                Name = "mytable",
                Schema = "dbo"
            },
            Data = new TableData { FileName = "dbo.mytable.data" },
            Columns = new List<IColumn>()
        };
        for (var i = 0; i < colCount; i++)
        {
            tableDefinition.Columns.Add(
                new SqlServerNVarChar(i + 1, $"col{i + 1}", true, 20));
        }

        ISelectCreator creator = new SelectCreator(_output);
        return creator.CreateSelect(tableDefinition).ToLowerInvariant();
    }
}