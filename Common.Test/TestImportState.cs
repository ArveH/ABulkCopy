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
    public async Task TestCreatingTablesInParallel_Then_ParentsFinishedBeforeChildren()
    {
        // Arrange
        const int noOfTables = 20;
        var allTables = new List<TableHolder>();
        var rndDelay = new Random(1);
        for (var i = 0; i < noOfTables; i++)
        {
            allTables.Add(new TableHolder($"table{i}", rndDelay.Next(10, 500)));
        }

        // Setting up Situation 4 (See DependencyGraphTests.cs)
        allTables[2].Parents.Add(allTables[0].Name, allTables[0]); allTables[0].Children.Add(allTables[2].Name, allTables[2]);
        allTables[2].Parents.Add(allTables[4].Name, allTables[4]); allTables[4].Children.Add(allTables[2].Name, allTables[2]);
        allTables[3].Parents.Add(allTables[1].Name, allTables[1]); allTables[1].Children.Add(allTables[3].Name, allTables[3]);
        allTables[4].Parents.Add(allTables[3].Name, allTables[3]); allTables[3].Children.Add(allTables[4].Name, allTables[4]);

        for (var i = 5; i < noOfTables-1; i++)
        {
            // 30% of tables depends on next table
            if (i % 3 == 0)
            {
                allTables[i].Parents.Add(allTables[i + 1].Name, allTables[i + 1]);
                allTables[i + 1].Children.Add(allTables[i].Name, allTables[i]);
            }
        }

        var creationOrderQueue = new ConcurrentQueue<string>();
        IImportState importState = new ImportState(
            allTables.Where(t => !t.IsIndependent),
            allTables.Where(t => t.IsIndependent), 
            TestLogger);

        await Parallel.ForEachAsync(
            importState.GetTablesReadyForCreation(),
            async (node, token) =>
            {
                var tableHolder = allTables.First(t => t.Name == node.Name);
                await Task.Delay(tableHolder.SimulatedCopyTime, token);
                importState.TableFinished(node);
                creationOrderQueue.Enqueue(node.Name);
            });

        var creationOrderList = creationOrderQueue.ToList();
        ValidateParentsCreatedFirst(allTables, creationOrderList);
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
    public TableDefinition? Value => throw new NotImplementedException();
    public bool IsRoot => Name == "root";
    public bool IsIndependent => Parents.Count == 0;
    public Dictionary<string, INode> Parents { get; } = new();
    public Dictionary<string, INode> Children { get; } = new();

    public void Accept(IVisitor visitor, int depth)
    {
        throw new NotImplementedException();
    }
}
