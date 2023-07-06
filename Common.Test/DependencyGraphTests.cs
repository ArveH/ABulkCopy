namespace Common.Test;

public class DependencyGraphTests
{
    [Fact]
    public void TestInitialize_When_Empty()
    {
        // Act
        var graph = GetDependencyGraph();

        // Assert
        graph.Count().Should().Be(0);
    }

    [Fact]
    public void TestAdd_When_InsertFirst()
    {
        // Arrange
        var tableName = "A1";
        var graph = GetDependencyGraph();
        var expected = new List<TableDefinition>()
        {
            MssTestData.GetEmpty(tableName)
        };

        // Act
        graph.Add(expected[0]);

        // Assert
        graph.Count().Should().Be(1, "because count is 1");
        var depths = graph.TableDepths();
        depths.Count.Should().Be(1, "because there is 1 table");
        depths[0].Name.Should().Be(tableName, $"because the table name is '{tableName}'");
        depths[0].Depth.Should().Be(1, $"because '{tableName}' is at level 1");
    }

    [Fact]
    public void TestAdd_When_Insert_Independent_Second()
    {
        // Arrange
        var tableName = "A1";
        var tableName2 = "B1";
        var graph = GetDependencyGraph();
        var expected = new List<TableDefinition>()
        {
            MssTestData.GetEmpty(tableName),
            MssTestData.GetEmpty(tableName2)
        };

        // Act
        graph.Add(expected[0]);
        graph.Add(expected[1]);

        // Assert
        graph.Count().Should().Be(2);
        var depths = graph.TableDepths();
        depths.Count.Should().Be(2, "because there are 2 tables");
        depths[0].Name.Should().Be(tableName, $"because the table name is '{tableName}'");
        depths[0].Depth.Should().Be(1, $"because '{tableName}' is at level 1");
        depths[1].Name.Should().Be(tableName2, $"because the table name is '{tableName2}'");
        depths[1].Depth.Should().Be(1, $"because '{tableName2}' is at level 1");
    }

    [Fact]
    public void Insert_Parent_Before_Child()
    {
        // Arrange
        var tableName = "A1";
        var tableName2 = "B2";
        var graph = GetDependencyGraph();
        var parent = MssTestData.GetEmpty(tableName);
        parent.Columns.Add(new SqlServerBigInt(1, "Id", false));
        var child = MssTestData.GetEmpty(tableName2);
        child.Columns.Add(new SqlServerBigInt(1, "Id", false));
        child.Columns.Add(new SqlServerBigInt(1, $"{tableName}_Id", false));
        child.ForeignKeys.Add(new ForeignKey
        {
            Name = $"FK_{tableName}_Id",
            ColName = $"{tableName}_Id",
            TableReference = parent.Header.Name,
            ColumnReference = parent.Columns.First().Name
        });

        // Act
        graph.Add(parent);
        graph.Add(child);

        // Assert
        graph.Count().Should().Be(2, "because Count should be 2");
        var depths = graph.TableDepths();
        depths.Count.Should().Be(2, "because there are 2 tables");
        depths[0].Name.Should().Be(tableName, $"because the table name is '{tableName}'");
        depths[0].Depth.Should().Be(1, $"because '{tableName}' is at level 1");
        depths[1].Name.Should().Be(tableName2, $"because the table name is '{tableName2}'");
        depths[1].Depth.Should().Be(2, $"because '{tableName2}' is at level 2");
    }

    [Fact]
    public void Insert_Child_Before_Parent()
    {
        // Arrange
        var tableName = "A1";
        var tableName2 = "B2";
        var graph = GetDependencyGraph();
        var parent = MssTestData.GetEmpty(tableName);
        parent.Columns.Add(new SqlServerBigInt(1, "Id", false));
        var child = MssTestData.GetEmpty(tableName2);
        child.Columns.Add(new SqlServerBigInt(1, "Id", false));
        child.Columns.Add(new SqlServerBigInt(1, $"{tableName}_Id", false));
        child.ForeignKeys.Add(new ForeignKey
        {
            Name = $"FK_{tableName}_Id",
            ColName = $"{tableName}_Id",
            TableReference = parent.Header.Name,
            ColumnReference = parent.Columns.First().Name
        });

        // Act
        graph.Add(child);
        graph.Add(parent);

        // Assert
        graph.Count().Should().Be(2);
        var depths = graph.TableDepths();
        depths.Count.Should().Be(2, "because there are 2 tables");
        depths[0].Name.Should().Be(tableName, $"because the table name is '{tableName}'");
        depths[0].Depth.Should().Be(1, $"because '{tableName}' is at level 1");
        depths[1].Name.Should().Be(tableName2, $"because the table name is '{tableName2}'");
        depths[1].Depth.Should().Be(2, $"because '{tableName2}' is at level 2");
    }

    private IDependencyGraph GetDependencyGraph()
    {
        return new DependencyGraph(new VisitorFactory());
    }
}