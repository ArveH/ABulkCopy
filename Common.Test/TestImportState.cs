using System.Collections.Concurrent;
using ABulkCopy.Common.Graph.Visitors;

namespace Common.Test;

public class TestImportState : CommonTestBase
{
    public TestImportState(ITestOutputHelper output)
        :base(output)
    {
    }

    [Fact]
    public async Task TestCreatingTablesInParallel_When_NoParents()
    {
        // Arrange
        const int noOfTables = 20;
        var allTables = CreateAllTables(noOfTables);
        var creationOrderQueue = new ConcurrentQueue<string>();
        IImportState importState = new ImportState(
            allTables.Where(t => !t.IsIndependent),
            allTables.Where(t => t.IsIndependent),
            TestLogger);

        // Act
        await Parallel.ForEachAsync(
            importState.GetTablesReadyForCreation(),
            async (node, token) =>
            {
                var tableHolder = allTables.First(t => t.Name == node.Name);
                await Task.Delay(tableHolder.SimulatedCopyTime, token);
                importState.TableFinished(node);
                creationOrderQueue.Enqueue(node.Name);
            });

        // Assert
        var actualTableNames = creationOrderQueue.ToList();
        var expectedTableNames = allTables.Select(t => t.Name).ToList();
        actualTableNames.Should().BeEquivalentTo(expectedTableNames);
    }

    [Fact]
    public async Task TestCreatingTablesInParallel_Then_ParentsFinishedBeforeChildren()
    {
        // Arrange
        const int noOfTables = 20;
        var allTables = CreateAllTables(noOfTables);
        SetupSituation4(allTables);
        CreateMoreParents(allTables, 3, 5);

        var creationOrderQueue = new ConcurrentQueue<string>();
        IImportState importState = new ImportState(
            allTables.Where(t => !t.IsIndependent),
            allTables.Where(t => t.IsIndependent), 
            TestLogger);

        // Act
        await Parallel.ForEachAsync(
            importState.GetTablesReadyForCreation(),
            async (node, token) =>
            {
                var tableHolder = allTables.First(t => t.Name == node.Name);
                await Task.Delay(tableHolder.SimulatedCopyTime, token);
                importState.TableFinished(node);
                creationOrderQueue.Enqueue(node.Name);
            });

        // Assert
        var creationOrderList = creationOrderQueue.ToList();
        ValidateParentsCreatedFirst(allTables, creationOrderList);
    }

    private static void CreateMoreParents(List<TableHolder> tables, int frequency, int startAt=0)
    {
        for (var i = startAt; i < tables.Count - 1; i++)
        {
            if (i % frequency == 0)
            {
                tables[i].Parents.Add(tables[i + 1].Name, tables[i + 1]);
                tables[i + 1].Children.Add(tables[i].Name, tables[i]);
            }
        }
    }

    private static List<TableHolder> CreateAllTables(int noOfTables)
    {
        var allTables = new List<TableHolder>();
        var rndDelay = new Random(1);
        for (var i = 0; i < noOfTables; i++)
        {
            allTables.Add(new TableHolder($"table{i}", rndDelay.Next(10, 500)));
        }

        return allTables;
    }

    private static void SetupSituation4(List<TableHolder> allTables)
    {
        // Setting up Situation 4 (See DependencyGraphTests.cs)
        CreateDependency(allTables[0], allTables[2]);
        CreateDependency(allTables[4], allTables[2]);
        CreateDependency(allTables[1], allTables[3]);
        CreateDependency(allTables[3], allTables[4]);
    }

    private static void CreateDependency(TableHolder parent, TableHolder child)
    {
        parent.Children.Add(child.Name, child);
        child.Parents.Add(parent.Name, parent);
    }

    private void ValidateParentsCreatedFirst(List<TableHolder> allTables, List<string> creationOrderList)
    {
        foreach (var table in allTables.Where(t => !t.IsIndependent))
        {
            var tablePos = creationOrderList.IndexOf(table.Name);
            foreach (var parent in table.Parents)
            {
                var parentPos = creationOrderList.IndexOf(parent.Key);
                parentPos.Should().BeLessThan(tablePos, $"because Parent '{parent.Key}' of '{table.Name}' should be created before {table.Name}");
            }
        }
    }
}

public class TableHolder : INode
{
    public TableHolder(string name, int delay)
    {
        Name = name;
        SimulatedCopyTime = delay;
    }

    public int SimulatedCopyTime { get; set; }

    public string Name { get; }
    public TableDefinition? TableDefinition => throw new NotImplementedException();
    public bool IsRoot => Name == "root";
    public bool IsIndependent => Parents.Count == 0;
    public Dictionary<string, INode> Parents { get; } = new();
    public Dictionary<string, INode> Children { get; } = new();

    public void Accept(IVisitor visitor, int depth)
    {
        throw new NotImplementedException();
    }
}
