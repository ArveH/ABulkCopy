using System.Collections.Concurrent;

namespace Common.Test;

public class TestImportState : CommonTestBase
{
    public TestImportState(ITestOutputHelper output)
        :base(output)
    {
    }

    [Fact]
    public void TestInitialization()
    {
        var allTables = new List<TableHolder>();
        var rndDelay = new Random(1);
        for (var i = 0; i < 100; i++)
        {
            allTables.Add(new TableHolder($"table{i}", rndDelay.Next(100, 2000)));
        }

        // See DependencyGraphTests --> Situation 4
        allTables[2].Parent1 = allTables[0].Name; allTables[0].Child = allTables[2].Name;
        allTables[2].Parent2 = allTables[4].Name; allTables[4].Child = allTables[2].Name;
        allTables[3].Parent1 = allTables[1].Name; allTables[1].Child = allTables[3].Name;
        allTables[4].Parent1 = allTables[3].Name; allTables[3].Child = allTables[4].Name;

        var rndChild = new Random(2);
        for (var i = 5; i < 100-1; i++)
        {
            if (i % 3 == 0)
            {
                allTables[i].Parent1 = allTables[i+1].Name; 
                allTables[i+1].Child = allTables[i].Name;
            }
        }

        var orderOfCreation = new ConcurrentQueue<string>();
        IImportState importState = new ImportState(
            allTables.Where(t => t.HasParent).Select(t => t.Name),
            allTables.Where(t => !t.HasParent).Select(t => t.Name), 
            TestLogger);

        Parallel.ForEachAsync(
            importState.GetTablesReadyForCreation(),
            async (tabDef, _) =>
            {
                var tableHolder = allTables.First(t => t.Name == tabDef);
                await Task.Delay(tableHolder.SimulatedCopyTime, _);
                importState.TableFinished(tabDef);
                orderOfCreation.Enqueue(tabDef);
            });
    }

}

public class TableHolder
{
    public TableHolder(
        string name, int delay)
    {
        Name = name;
        SimulatedCopyTime = delay;
    }

    public bool HasParent => Parent1 != null || Parent2 != null;
    public string Name { get; set; }
    public string? Parent1 { get; set; }
    public string? Parent2 { get; set; }
    public string? Child { get; set; }
    public int SimulatedCopyTime { get; set; }
}