namespace Common.Test;

public class TreeTests
{
    private readonly TreeDoctor _treeDoctor;

    public TreeTests()
    {
        _treeDoctor = new TreeDoctor();
    }

    [Fact]
    public void TestAdd_When_RootEmpty()
    {
        // Arrange
        var root = new Root();
        var tabDef = MssTestData.GetEmpty("A_");

        // Act
        _treeDoctor.Add(root, tabDef);

        // Assert
        root.Children.Count.Should().Be(1);
        root.Children[0].Name.Should().Be("A_");
        root.Children[0].Value.Should().BeEquivalentTo(tabDef);
    }
}