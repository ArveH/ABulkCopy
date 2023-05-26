namespace ASqlServer.Test;

public class MssIndexTests : MssTestBase
{
    public MssIndexTests(ITestOutputHelper output) 
        : base(output)
    {
    }

    [Fact]
    public async Task TestGetIndexes_When_OneIndexOneColumn()
    {
        // Arrange
        var systemTables = CreateMssSystemTables();
        var tableHeader = await systemTables.GetTableHeader("ConfiguredClients");
        tableHeader.Should().NotBeNull();

        // Act
        var indexes = (await systemTables.GetIndexes(tableHeader!)).ToList();

        // Assert
        indexes.Should().NotBeNull();
        indexes.Count.Should().Be(1);
        indexes[0].Header.Id.Should().Be(2);
        indexes[0].Header.TableId.Should().BeGreaterThan(0);
        indexes[0].Header.Name.Should().Be("IX_ConfiguredClients_OwnerTenant");
        indexes[0].Header.IsUnique.Should().BeFalse();
        indexes[0].Header.Type.Should().Be(IndexType.NonClustered);
        indexes[0].Header.Location.Should().Be("PRIMARY");
    }


}