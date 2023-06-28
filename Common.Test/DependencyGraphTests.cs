namespace Common.Test;

public class DependencyGraphTests
{
    [Fact]
    public void TestInitialize_When_Empty()
    {
        // Act
        var graph = new DependencyGraph(new List<TableDefinition>());

        // Assert
        graph.Count.Should().Be(0);
        graph.TablesInOrder.Count().Should().Be(0);
    }

    [Fact]
    public void TestInitialize_When_First()
    {
        // Arrange
        var tableDefinitions = new List<TableDefinition>()
        {
            MssTestData.GetEmpty("A_")
        };

        // Act
        var graph = new DependencyGraph(tableDefinitions);

        // Assert
        graph.Count.Should().Be(1);
        graph.TablesInOrder.Should().BeEquivalentTo(tableDefinitions);
    }
}