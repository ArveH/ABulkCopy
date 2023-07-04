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
        graph.TablesInOrder.Should().BeEmpty();
    }

    [Fact]
    public void TestAdd_When_InsertFirst()
    {
        // Arrange
        var graph = GetDependencyGraph();
        var expected = new List<TableDefinition>()
        {
            MssTestData.GetEmpty("A")
        };

        // Act
        graph.Add(expected[0]);

        // Assert
        graph.Count().Should().Be(1);
        graph.TablesInOrder.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void TestAdd_When_Insert_Independent_Second()
    {
        // Arrange
        var graph = GetDependencyGraph();
        var expected = new List<TableDefinition>()
        {
            MssTestData.GetEmpty("A"),
            MssTestData.GetEmpty("B")
        };

        // Act
        graph.Add(expected[0]);
        graph.Add(expected[1]);

        // Assert
        graph.Count().Should().Be(2);
        graph.TablesInOrder.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Insert_Parent_Before_Child()
    {
        // Arrange
        var graph = GetDependencyGraph();
        var parent = MssTestData.GetEmpty("A");
        parent.Columns.Add(new SqlServerBigInt(1, "Id", false));
        var child = MssTestData.GetEmpty("B_Depends_On_A");
        child.Columns.Add(new SqlServerBigInt(1, "Id", false));
        child.Columns.Add(new SqlServerBigInt(1, "A_Id", false));
        child.ForeignKeys.Add(new ForeignKey
        {
            Name = "FK_A_Id",
            ColName = "A_Id",
            TableReference = parent.Header.Name,
            ColumnReference = parent.Columns.First().Name
        });

        // Act
        graph.Add(parent);
        graph.Add(child);

        // Assert
        graph.Count().Should().Be(2, "because Count should be 2");
        var tablesInOrder = graph.TablesInOrder.ToList();
        tablesInOrder.Count.Should().Be(2, "because tablesInOrder should be 2");
        tablesInOrder[0].Should().BeEquivalentTo(parent);
        tablesInOrder[1].Should().BeEquivalentTo(child);
    }

    [Fact]
    public void Insert_Child_Before_Parent()
    {
        // Arrange
        var graph = GetDependencyGraph();
        var parent = MssTestData.GetEmpty("A_B");
        parent.Columns.Add(new SqlServerBigInt(1, "Id", false));
        var child = MssTestData.GetEmpty("B_");
        child.Columns.Add(new SqlServerBigInt(1, "Id", false));
        child.Columns.Add(new SqlServerBigInt(1, "A_Id", false));
        child.ForeignKeys.Add(new ForeignKey
        {
            Name = "FK_A_Id",
            ColName = "A_Id",
            TableReference = parent.Header.Name,
            ColumnReference = parent.Columns.First().Name
        });

        // Act
        graph.Add(child);
        graph.Add(parent);

        // Assert
        graph.Count().Should().Be(2);
        var tablesInOrder = graph.TablesInOrder.ToList();
        tablesInOrder.Count.Should().Be(2);
        tablesInOrder[0].Should().BeEquivalentTo(parent);
        tablesInOrder[1].Should().BeEquivalentTo(child);
    }

    private IDependencyGraph GetDependencyGraph()
    {
        return new DependencyGraph(new VisitorFactory());
    }
}