namespace Common.Tests;

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
            tableDefinitions.Select(d => d.GetFullName()).ToList(),
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
            tableDefinitions.Select(d => d.GetFullName()).ToList(),
            new List<int> { 1, 1 });
    }

    [Fact]
    public void TestAdd_When_InsertParentBeforeChild()
    {
        // Arrange
        var tableDefinitions = GetTableDefinitions("A1", "B2");
        SetForeignKey(tableDefinitions[0].Header.Name, tableDefinitions[1]);
        var graph = GetDependencyGraph();

        // Act
        graph.Add(tableDefinitions[0]);
        graph.Add(tableDefinitions[1]);

        // Assert
        VerifyCount(graph, tableDefinitions.Count);
        VerifyDepths(
            graph.GetTableDepths(),
            tableDefinitions.Select(d => d.GetFullName()).ToList(),
            new List<int> { 1, 2 });
    }

    [Fact]
    public void TestAdd_When_InsertChildBeforeParent()
    {
        // Arrange
        var tableDefinitions = GetTableDefinitions("A1", "B2");
        SetForeignKey(tableDefinitions[0].Header.Name, tableDefinitions[1]);
        var graph = GetDependencyGraph();

        // Act
        graph.Add(tableDefinitions[1]);
        graph.Add(tableDefinitions[0]);

        // Assert
        VerifyCount(graph, tableDefinitions.Count);
        VerifyDepths(
            graph.GetTableDepths(),
            tableDefinitions.Select(d => d.GetFullName()).ToList(),
            new List<int> { 1, 2 });
    }

    // Situation 1: Both dependencies above in the tree
    // c depends on a and b
    //   - child is inserted before one of the parents
    //         a     b                       
    //               
    //         ^     ^                       
    //         |     |
    //               
    //            c
    //               
    [Fact]
    public void TestOrder_When_Situation1()
    {
        // Arrange
        var tableDefinitions = GetTableDefinitions("a", "c", "b");
        SetForeignKey(tableDefinitions[0].Header.Name, tableDefinitions[1]);
        SetForeignKey(tableDefinitions[2].Header.Name, tableDefinitions[1]);
        var graph = GetDependencyGraph();
        tableDefinitions.ForEach(graph.Add);

        // Act
        var result = graph.GetTablesInOrder().ToList();

        // Assert
        result.Count.Should().Be(tableDefinitions.Count);
        VerifyFirstLevel(result, "a", "b");
        result[2].Header.Name.Should().Be("c");
    }

    private void VerifyFirstLevel(List<TableDefinition> result, string t1, string t2)
    {
        result[0].Header.Name.Should().BeOneOf(t1, t2);
        result[1].Header.Name.Should().BeOneOf(t1, t2);
        result[0].Should().NotBe(result[1]);
    }

    // Situation 2: Both dependencies above in the tree, but on different levels
    // d depends on c
    // d depends on b
    // c depends on a
    //
    //    a        b                       
    //               
    //    ^        ^                       
    //    |        |    
    //            |     
    //    c      |                         
    //          |       
    //    ^    |                          
    //    |   |                            
    //                  
    //      d                                 
    //                                                 
    [Fact]
    public void TestOrder_When_Situation2()
    {
        // Arrange
        var tableDefinitions = GetTableDefinitions("d", "a", "b", "c");
        SetForeignKey(tableDefinitions[3].Header.Name, tableDefinitions[0]);
        SetForeignKey(tableDefinitions[2].Header.Name, tableDefinitions[0]);
        SetForeignKey(tableDefinitions[1].Header.Name, tableDefinitions[3]);
        var graph = GetDependencyGraph();
        tableDefinitions.ForEach(graph.Add);

        // Act
        var result = graph.GetTablesInOrder().ToList();

        // Assert
        result.Count.Should().Be(tableDefinitions.Count);
        VerifyFirstLevel(result, "a", "b");
        result[2].Header.Name.Should().Be("c");
        result[3].Header.Name.Should().Be("d");
    }

    // Situation 3: One dependency above, and one at the same level
    // c depends on a
    // d depends on b
    // c depends on d
    //      a      b                       
    //             
    //      ^      ^                       
    //      |      |
    //             
    //      c  ->  d                       
    //                                                
    [Fact]
    public void TestOrder_When_Situation3()
    {
        // Arrange
        var tableDefinitions = GetTableDefinitions("a", "b", "c", "d");
        SetForeignKey(tableDefinitions[0].Header.Name, tableDefinitions[2]);
        SetForeignKey(tableDefinitions[1].Header.Name, tableDefinitions[3]);
        SetForeignKey(tableDefinitions[3].Header.Name, tableDefinitions[2]);
        var graph = GetDependencyGraph();
        tableDefinitions.ForEach(graph.Add);

        // Act
        var result = graph.GetTablesInOrder().ToList();

        // Assert
        result.Count.Should().Be(tableDefinitions.Count);
        VerifyFirstLevel(result, "a", "b");
        result[2].Header.Name.Should().Be("d");
        result[3].Header.Name.Should().Be("c");
    }

    // Situation 4: One dependency above, and one below
    // c depends on a and e
    // d depends on b
    // e depends on d
    //         a     b                       
    //               
    //         ^     ^                       
    //         |     |
    //               
    //         c     d                       
    //               
    //           \   ^                      
    //            v  |                      
    //               
    //               e                      
    //                                                 
    [Fact]
    public void TestOrder_When_Situation4()
    {
        // Arrange
        var tableDefinitions = GetTableDefinitions("a", "b", "c", "d", "e");
        SetForeignKey(tableDefinitions[0].Header.Name, tableDefinitions[2]);
        SetForeignKey(tableDefinitions[4].Header.Name, tableDefinitions[2]);
        SetForeignKey(tableDefinitions[1].Header.Name, tableDefinitions[3]);
        SetForeignKey(tableDefinitions[3].Header.Name, tableDefinitions[4]);
        var graph = GetDependencyGraph();
        tableDefinitions.ForEach(graph.Add);

        // Act
        var result = graph.GetTablesInOrder().ToList();

        // Assert
        result.Count.Should().Be(tableDefinitions.Count);
        VerifyFirstLevel(result, "a", "b");
        result[2].Header.Name.Should().Be("d");
        result[3].Header.Name.Should().Be("e");
        result[4].Header.Name.Should().Be("c");
    }

    // Situation 5: Same as 4, but tables inserted in different order
    // c depends on a and e
    // d depends on b
    // e depends on d
    //         a     b                       
    //               
    //         ^     ^                       
    //         |     |
    //               
    //         c     d                       
    //               
    //           \   ^                      
    //            v  |                      
    //               
    //               e                      
    //                                                 
    [Fact]
    public void TestOrder_When_Situation5()
    {
        // Arrange
        var tableDefinitions = GetTableDefinitions("c", "b", "a", "e", "d");
        SetForeignKey(tableDefinitions[1].Header.Name, tableDefinitions[0]);
        SetForeignKey(tableDefinitions[3].Header.Name, tableDefinitions[0]);
        SetForeignKey(tableDefinitions[1].Header.Name, tableDefinitions[4]);
        SetForeignKey(tableDefinitions[4].Header.Name, tableDefinitions[3]);
        var graph = GetDependencyGraph();
        tableDefinitions.ForEach(graph.Add);

        // Act
        var result = graph.GetTablesInOrder().ToList();

        // Assert
        result.Count.Should().Be(tableDefinitions.Count);
        VerifyFirstLevel(result, "a", "b");
        result[2].Header.Name.Should().Be("d");
        result[3].Header.Name.Should().Be("e");
        result[4].Header.Name.Should().Be("c");
    }

    private static void VerifyCount(IDependencyGraph graph, int expectedCount)
    {
        graph.Count().Should().Be(
            expectedCount,
            $"because Count should be {expectedCount}");
    }

    private void SetForeignKey(string parentName, TableDefinition child)
    {
        child.Columns.Add(new SqlServerBigInt(1, $"{parentName}_Id", false));
        child.ForeignKeys.Add(new ForeignKey
        {
            ConstraintName = $"FK_{parentName}_Id",
            ColumnNames = new List<string> { $"{parentName}_Id" },
            SchemaReference = "dbo",
            TableReference = parentName,
            ColumnReferences = new List<string> { "Id" },
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
            var tabDef = MssTestData.GetEmpty(("dbo", name));
            tabDef.Columns.Add(new SqlServerBigInt(1, "Id", false));
            tableDefinitions.Add(tabDef);
        }

        return tableDefinitions;
    }

    private IDependencyGraph GetDependencyGraph()
    {
        return new DependencyGraph(
            new NodeFactory(),
            new VisitorFactory());
    }
}