namespace Postgres.Tests.DataReader;

[Collection(nameof(DatabaseCollection))]
public class PgDataReaderTestBase : PgTestBase
{
    protected readonly FileHelper FileHelper = new ();
    protected readonly Mock<IQueryBuilderFactory> QBFactoryMock = new();

    protected PgDataReaderTestBase(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
        QBFactoryMock.Setup(f => f.GetQueryBuilder())
            .Returns(() => new QueryBuilder(
                new Identifier(dbFixture.Configuration, DbFixture.PgContext)));
    }

    protected async Task<T?> TestDataReader<T>(
        string tableName,
        IColumn col,
        string fileData)
    {
        try
        {
            // Arrange
            await CreateTableAndReadData(
                tableName, new() { col }, new() { fileData });

            // Assert
            return await DbFixture.SelectScalar<T>(
                ("public", tableName), col);
        }
        finally
        {
            await DbFixture.DropTable(("public", tableName));
        }
    }

    protected async Task CreateTableAndReadData(
        string tableName,
        List<IColumn> cols,
        List<string> fileData)
    {
        var tableDefinition = await CreateTableAndDataFile(("public", tableName), cols, fileData);
        var dataReader = new PgDataReader(
            DbFixture.PgContext,
            QBFactoryMock.Object,
            new DataFileReader(FileHelper.FileSystem, TestLogger), 
            TestLogger);
        var cts = new CancellationTokenSource();
        await dataReader.ReadAsync(FileHelper.DataFolder, tableDefinition, cts.Token);
    }

    protected async Task<TableDefinition> CreateTableAndDataFile(
        SchemaTableTuple st, 
        List<IColumn> cols, 
        List<string> fileData)
    {
        var tableDefinition = PgTestData.GetEmpty(st);
        cols.ForEach(tableDefinition.Columns.Add);
        await DbFixture.DropTable(st);
        await DbFixture.CreateTable(tableDefinition);
        FileHelper.CreateDataFile(st, fileData);
        return tableDefinition;
    }
}