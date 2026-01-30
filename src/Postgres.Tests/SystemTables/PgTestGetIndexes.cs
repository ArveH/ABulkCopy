namespace Postgres.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class PgTestGetIndexes : PgTestBase, IAsyncLifetime
{
    private readonly CancellationTokenSource _cts = new();
    private string _tableName = string.Empty;
    private readonly IIdentifier _identifier;

    public PgTestGetIndexes(DatabaseFixture dbFixture, ITestOutputHelper output) 
        : base(dbFixture, output)
    {
        _identifier = GetIdentifier();
    }
    
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await DropTableAsync(DatabaseFixture.DefaultSchemaName, _tableName);
    }

    [Theory]
    [InlineData(IndexType.BTree, "USING btree (col1)")]
    [InlineData(IndexType.BTree, "(col1)")]
    [InlineData(IndexType.Hash, "USING hash (col1)")]
    [InlineData(IndexType.Gist, "USING gist (to_tsvector('english', text_col))")]
    [InlineData(IndexType.Gin, "USING gin (to_tsvector('english', text_col))")]
    [InlineData(IndexType.Brin, "USING brin (col1)")]
    [InlineData(IndexType.SpGist, "USING spgist (text_col)")]
    public async Task TestGetIndexes_When_Type(IndexType expectedType, string usingClause)
    {
        // Arrange
        _tableName = GetName();
        var tableHeader = await CreateTestTable();
        
        var indexName = tableHeader!.Name.ToLower() + "_idx";
        await ExecuteNonQueryAsync(
            $"CREATE INDEX {indexName} ON {tableHeader.Name.ToLower()} {usingClause}");

        // Act
        var indexes = (await PgSystemTables.GetIndexesAsync(tableHeader, _cts.Token)).ToList();

        // Assert
        VerifyStandardIndexValues(indexes[0]);
        indexes.Count.Should().Be(1);
        indexes[0].Header.Name.Should().Be(_identifier.AdjustForSystemTable(indexName));
        indexes[0].Header.IsUnique.Should().BeFalse();
        indexes[0].Header.Type.Should().Be(expectedType);
    }

    [Fact]
    public async Task TestGetIndexes_When_BTreeIndex_MultipleColumns()
    {
        // Arrange
        _tableName = GetName();
        var tableHeader = await CreateTestTable();
        
        var indexName = tableHeader!.Name.ToLower() + "_btree_multi_idx";
        await ExecuteNonQueryAsync(
            $"CREATE INDEX {indexName} ON {tableHeader.Name.ToLower()} USING btree (col1, col2, col3)");

        // Act
        var indexes = (await PgSystemTables.GetIndexesAsync(tableHeader, _cts.Token)).ToList();

        // Assert
        VerifyStandardIndexValues(indexes[0]);
        indexes.Count.Should().Be(1);
        indexes[0].Header.Name.Should().Be(_identifier.AdjustForSystemTable(indexName));
        indexes[0].Header.Type.Should().Be(IndexType.BTree);
        indexes[0].Header.IsUnique.Should().BeFalse();
    }

    [Fact]
    public async Task TestGetIndexes_When_UniqueIndex()
    {
        // Arrange
        _tableName = GetName();
        var tableHeader = await CreateTestTable();
        
        var indexName = tableHeader!.Name.ToLower() + "_unique_idx";
        await ExecuteNonQueryAsync(
            $"CREATE UNIQUE INDEX {indexName} ON {tableHeader.Name.ToLower()} (col1)");

        // Act
        var indexes = (await PgSystemTables.GetIndexesAsync(tableHeader, _cts.Token)).ToList();

        // Assert
        VerifyStandardIndexValues(indexes[0]);
        indexes.Count.Should().Be(1);
        indexes[0].Header.Name.Should().Be(_identifier.AdjustForSystemTable(indexName));
        indexes[0].Header.IsUnique.Should().BeTrue();
        indexes[0].Header.Type.Should().Be(IndexType.BTree);
    }

    [Fact]
    public async Task TestGetIndexes_When_MultipleIndexes()
    {
        // Arrange
        _tableName = GetName();
        var tableHeader = await CreateTestTable();
        
        var indexName1 = tableHeader!.Name.ToLower() + "_idx1";
        var indexName2 = tableHeader.Name.ToLower() + "_idx2";
        var indexName3 = tableHeader.Name.ToLower() + "_idx3";
        
        await ExecuteNonQueryAsync($"CREATE INDEX {indexName1} ON {tableHeader.Name.ToLower()} (col1)");
        await ExecuteNonQueryAsync($"CREATE INDEX {indexName2} ON {tableHeader.Name.ToLower()} (col2)");
        await ExecuteNonQueryAsync($"CREATE UNIQUE INDEX {indexName3} ON {tableHeader.Name.ToLower()} (col3)");

        // Act
        var indexes = (await PgSystemTables.GetIndexesAsync(tableHeader, _cts.Token)).ToList();

        // Assert
        indexes.Should().NotBeNull();
        indexes.Count.Should().Be(3);
        indexes.Should().Contain(i => i.Header.Name == _identifier.AdjustForSystemTable(indexName1));
        indexes.Should().Contain(i => i.Header.Name == _identifier.AdjustForSystemTable(indexName2));
        indexes.Should().Contain(i => i.Header.Name == _identifier.AdjustForSystemTable(indexName3));
        
        var uniqueIndex = indexes.First(i => i.Header.Name == _identifier.AdjustForSystemTable(indexName3));
        uniqueIndex.Header.IsUnique.Should().BeTrue();
    }

    [Fact]
    public async Task TestGetIndexes_When_NoIndexes()
    {
        // Arrange
        _tableName = GetName();
        var tableHeader = await CreateTestTable();

        // Act
        var indexes = (await PgSystemTables.GetIndexesAsync(tableHeader!, _cts.Token)).ToList();

        // Assert
        indexes.Should().NotBeNull();
        indexes.Count.Should().Be(0);
    }

    [Fact]
    public async Task TestGetIndexes_When_PartialIndex()
    {
        // Arrange
        _tableName = GetName();
        var tableHeader = await CreateTestTable();
        
        var indexName = tableHeader!.Name.ToLower() + "_partial_idx";
        await ExecuteNonQueryAsync(
            $"CREATE INDEX {indexName} ON {tableHeader.Name.ToLower()} (col1) WHERE col1 > 0");

        // Act
        var indexes = (await PgSystemTables.GetIndexesAsync(tableHeader, _cts.Token)).ToList();

        // Assert
        indexes.Should().NotBeNull();
        indexes.Count.Should().Be(1);
        indexes[0].Header.Name.Should().Be(_identifier.AdjustForSystemTable(indexName));
        indexes[0].Header.Type.Should().Be(IndexType.BTree);
    }

    [Fact]
    public async Task TestGetIndexes_When_ExpressionIndex()
    {
        // Arrange
        _tableName = GetName();
        var tableHeader = await CreateTestTable();
        
        var indexName = tableHeader!.Name.ToLower() + "_expr_idx";
        await ExecuteNonQueryAsync(
            $"CREATE INDEX {indexName} ON {tableHeader.Name.ToLower()} (LOWER(text_col))");

        // Act
        var indexes = (await PgSystemTables.GetIndexesAsync(tableHeader, _cts.Token)).ToList();

        // Assert
        indexes.Should().NotBeNull();
        indexes.Count.Should().Be(1);
        indexes[0].Header.Name.Should().Be(_identifier.AdjustForSystemTable(indexName));
        indexes[0].Header.Type.Should().Be(IndexType.BTree);
        indexes[0].Columns.Count.Should().Be(1);
        indexes[0].Columns[0].Name.Should().Be("lower(text_col)");
    }

    [Fact]
    public async Task TestGetIndexes_When_ClusteredIndex()
    {
        // Arrange
        _tableName = GetName();
        var tableHeader = await CreateTestTable();
        
        var indexName = tableHeader!.Name.ToLower() + "_cluster_idx";
        await ExecuteNonQueryAsync($"CREATE INDEX {indexName} ON {tableHeader.Name.ToLower()} (col1)");
        await ExecuteNonQueryAsync($"CLUSTER {tableHeader.Name.ToLower()} USING {indexName}");

        // Act
        var indexes = (await PgSystemTables.GetIndexesAsync(tableHeader, _cts.Token)).ToList();

        // Assert
        indexes.Should().NotBeNull();
        indexes.Count.Should().Be(1);
        indexes[0].Header.Name.Should().Be(_identifier.AdjustForSystemTable(indexName));
        indexes[0].Header.IsClustered.Should().BeTrue();
        indexes[0].Header.Type.Should().Be(IndexType.BTree);
    }

    private async Task<TableHeader?> CreateTestTable()
    {
        await DropTableAsync(DatabaseFixture.DefaultSchemaName, _tableName);

        var tableDef = PgTestData.GetEmpty((DatabaseFixture.DefaultSchemaName, _tableName));
        tableDef.Columns.Add(new PostgresInt(1, "col1", false));
        tableDef.Columns.Add(new PostgresInt(2, "col2", false));
        tableDef.Columns.Add(new PostgresInt(3, "col3", false));
        tableDef.Columns.Add(new PostgresText(4, "text_col", false));
        await DbFixture.CreateTable(tableDef);
        var tableHeader = await PgSystemTables.GetTableHeaderAsync(
            DatabaseFixture.DefaultSchemaName, _tableName, _cts.Token);
        
        tableHeader.Should().NotBeNull("because table should exist");
        return tableHeader;
    }

    private void VerifyStandardIndexValues(IndexDefinition? indexDefinition)
    {
        indexDefinition.Should().NotBeNull();
        indexDefinition!.Header.Id.Should().BeGreaterThan(0);
        indexDefinition.Header.TableId.Should().BeGreaterThan(0);
        indexDefinition.Header.IsClustered.Should().BeFalse();
    }
}
