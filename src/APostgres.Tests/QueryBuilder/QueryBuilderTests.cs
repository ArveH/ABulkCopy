using ABulkCopy.APostgres;
using ABulkCopy.Common.Utils;

namespace APostgres.Tests.QueryBuilder;

public class QueryBuilderTests
{
    private readonly Mock<IDbContext> _dbContextMock = new();
    private const string TableName = "MyTable";
    private const string ColName = "MyCol";
    private const string FKName = "FKCol";
    private const string RefTableName = "ParentTable";
    private const string RefColName = "MyId";
    private IReadOnlyList<string> _allIdentifiers = 
    [
        TableName,
        ColName,
        FKName,
        RefTableName,
        RefColName
    ];

    public QueryBuilderTests()
    {
        _dbContextMock.Setup(c => c.Rdbms).Returns(Rdbms.Pg);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TestQueryBuilder_When_ToLowerIsTrue(bool addQuotes)
    {
        // Arrange
        var queryBuilderFactory = GetFactory(addQuotes, true);
        var qb = queryBuilderFactory.GetQueryBuilder();

        // Act
        var stmt = qb.CreateTableStmt(GetTableDefinition());

        // Assert
        stmt.Should().ContainAll(_allIdentifiers.Select(i => i.ToLower()));
        stmt.Should().NotContainAll(_allIdentifiers);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TestQueryBuilder_When_ToLowerIsFalse(bool addQuotes)
    {
        // Arrange
        var queryBuilderFactory = GetFactory(addQuotes, false);
        var qb = queryBuilderFactory.GetQueryBuilder();

        // Act
        var stmt = qb.CreateTableStmt(GetTableDefinition());

        // Assert
        stmt.Should().NotContainAll(_allIdentifiers.Select(i => i.ToLower()));
        stmt.Should().ContainAll(_allIdentifiers);
    }

    private IQueryBuilderFactory GetFactory(
        bool addQuotes,
        bool toLower)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new(Constants.Config.AddQuotes, addQuotes.ToString()),
                new(Constants.Config.ToLower, toLower.ToString())
            ])
            .Build();
        IIdentifier identifier = new Identifier(config, _dbContextMock.Object);
        return new QueryBuilderFactory(identifier);
    }

    private TableDefinition GetTableDefinition()
    {
        var tableDef = MssTestData.GetEmpty(("dbo", TableName));
        tableDef.Columns.Add(new SqlServerInt(1, ColName, false));
        tableDef.Columns.Add(new SqlServerInt(1, FKName, false));
        tableDef.PrimaryKey = new PrimaryKey
        {
            ColumnNames = [new OrderColumn { Name = ColName }]
        };
        tableDef.ForeignKeys.Add(new ForeignKey
        {
            ColumnNames = [FKName],
            ColumnReferences = [RefColName],
            SchemaReference = "dbo",
            TableReference = RefTableName
        });

        return tableDef;
    }
}