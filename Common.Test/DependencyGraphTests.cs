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
        var tableDefinitions = GetTableDefinitions("A1");
        var graph = GetDependencyGraph();

        // Act
        graph.Add(tableDefinitions[0]);

        // Assert
        VerifyCount(graph, tableDefinitions.Count);
        VerifyDepths(
            graph.GetTableDepths(),
            tableDefinitions.Select(d => d.Header.Name).ToList(),
            new List<int> { 1 });
    }

    [Fact]
    public void TestAdd_When_Insert_Independent_Second()
    {
        // Arrange
        var tableDefinitions = GetTableDefinitions("A1", "B1");
        var graph = GetDependencyGraph();

        // Act
        graph.Add(tableDefinitions[0]);
        graph.Add(tableDefinitions[1]);

        // Assert
        VerifyCount(graph, tableDefinitions.Count);
        VerifyDepths(
            graph.GetTableDepths(),
            tableDefinitions.Select(d => d.Header.Name).ToList(),
            new List<int> { 1, 1 });
    }

    [Fact]
    public void TestAdd_When_InsertParentBeforeChild()
    {
        // Arrange
        var tableDefinitions = GetTableDefinitions("A1", "B2");
        SetDependency(tableDefinitions[0].Header.Name, tableDefinitions[1]);
        var graph = GetDependencyGraph();

        // Act
        graph.Add(tableDefinitions[0]);
        graph.Add(tableDefinitions[1]);

        // Assert
        VerifyCount(graph, tableDefinitions.Count);
        VerifyDepths(
            graph.GetTableDepths(),
            tableDefinitions.Select(d => d.Header.Name).ToList(),
            new List<int> { 1, 2 });
    }

    [Fact]
    public void TestAdd_When_InsertChildBeforeParent()
    {
        // Arrange
        var tableDefinitions = GetTableDefinitions("A1", "B2");
        SetDependency(tableDefinitions[0].Header.Name, tableDefinitions[1]);
        var graph = GetDependencyGraph();

        // Act
        graph.Add(tableDefinitions[1]);
        graph.Add(tableDefinitions[0]);

        // Assert
        VerifyCount(graph, tableDefinitions.Count);
        VerifyDepths(
            graph.GetTableDepths(),
            tableDefinitions.Select(d => d.Header.Name).ToList(),
            new List<int> { 1, 2 });
    }

    private static void VerifyCount(IDependencyGraph graph, int expectedCount)
    {
        graph.Count().Should().Be(
            expectedCount,
            $"because Count should be {expectedCount}");
    }

    private void SetDependency(string parentName, TableDefinition child)
    {
        child.Columns.Add(new SqlServerBigInt(1, $"{parentName}_Id", false));
        child.ForeignKeys.Add(new ForeignKey
        {
            Name = $"FK_{parentName}_Id",
            ColName = $"{parentName}_Id",
            TableReference = parentName,
            ColumnReference = "Id"
        });
    }

    private static void VerifyDepths(
        IReadOnlyList<TableDepth> tableDepths,
        IReadOnlyList<string> tableNames,
        IReadOnlyList<int> expectedDepths)
    {
        tableDepths.Count.Should().Be(tableNames.Count);
        for (var i = 0; i < tableNames.Count; i++)
        {
            tableDepths[i].Name.Should().Be(
                tableNames[i],
                $"because the table name is '{tableNames[i]}'");
            tableDepths[i].Depth.Should().Be(
                expectedDepths[i],
                $"because '{tableNames[i]}' is at level {expectedDepths[i]}");
        }
    }

    private List<TableDefinition> GetTableDefinitions(params string[] names)
    {
        var tableDefinitions = new List<TableDefinition>();
        foreach (var name in names)
        {
            var tabDef = MssTestData.GetEmpty(name);
            tabDef.Columns.Add(new SqlServerBigInt(1, "Id", false));
            tableDefinitions.Add(tabDef);
        }

        return tableDefinitions;
    }

    private IDependencyGraph GetDependencyGraph()
    {
        return new DependencyGraph(new VisitorFactory());
    }
}