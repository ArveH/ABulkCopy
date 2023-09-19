using Testing.Shared.SqlServer;

namespace APostgres.Test.DataReader;

public class PgDataReaderTestBase : PgTestBase
{
    protected readonly FileHelper FileHelper = new ();

    protected PgDataReaderTestBase(ITestOutputHelper output)
        : base(output)
    {
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
            PgContext, 
            Quoter,
            new DataFileReader(FileHelper.FileSystem, TestLogger), 
            TestLogger);
        await dataReader.Read(FileHelper.DataFolder, tableDefinition);
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