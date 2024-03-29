namespace Postgres.DbTests.DataReader;

public class PgDataReaderTestBase : PgTestBase
{
    protected readonly FileHelper FileHelper = new ();
    protected readonly Mock<IQueryBuilderFactory> QBFactoryMock = new();

    protected PgDataReaderTestBase(ITestOutputHelper output)
        : base(output)
    {
        QBFactoryMock.Setup(f => f.GetQueryBuilder())
            .Returns(() => new QueryBuilder(
                new Identifier(TestConfiguration, PgDbHelper.Instance.PgContext)));
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
            return await PgDbHelper.Instance.SelectScalar<T>(
                tableName, col);
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(tableName);
        }
    }

    protected async Task CreateTableAndReadData(
        string tableName,
        List<IColumn> cols,
        List<string> fileData)
    {
        var tableDefinition = await CreateTableAndDataFile(tableName, cols, fileData);
        var dataReader = new PgDataReader(
            PgDbHelper.Instance.PgContext,
            QBFactoryMock.Object,
            new DataFileReader(FileHelper.FileSystem, TestLogger), 
            TestLogger);
        var cts = new CancellationTokenSource();
        await dataReader.ReadAsync(FileHelper.DataFolder, tableDefinition, cts.Token);
    }

    protected async Task<TableDefinition> CreateTableAndDataFile(
        string tableName, 
        List<IColumn> cols, 
        List<string> fileData)
    {
        var tableDefinition = MssTestData.GetEmpty(tableName);
        cols.ForEach(tableDefinition.Columns.Add);
        await PgDbHelper.Instance.DropTable(tableName);
        await PgDbHelper.Instance.CreateTable(tableDefinition);
        FileHelper.CreateDataFile(tableName, fileData);
        return tableDefinition;
    }
}