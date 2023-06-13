using System.Diagnostics;

namespace APostgres.Test.DataReader;

public class PgDataReaderTestBase : PgTestBase
{
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
        var tableDefinition = MssTestData.GetEmpty(tableName);
        cols.ForEach(col => tableDefinition.Columns.Add(col));
        var fileHelper = new FileHelper(tableName, DbServer.SqlServer);
        var dataFileReaderFactory = new DataFileReaderFactory(fileHelper.FileSystem, TestLogger);
        await PgDbHelper.Instance.DropTable(tableName);
        await PgDbHelper.Instance.CreateTable(tableDefinition);
        fileHelper.CreateDataFile(fileData);
        var dataReader = new PgDataReader(PgContext, dataFileReaderFactory, TestLogger);
        await dataReader.Read(fileHelper.DataFolder, tableDefinition);
    }

    //[MethodImpl(MethodImplOptions.NoInlining)]
    protected string GetName()
    {
        var st = new StackTrace();
        var sf = st.GetFrame(3);
        if (sf == null)
        {
            throw new InvalidOperationException("Stack Frame is null");
        }

        return sf.GetMethod()?.Name ?? throw new InvalidOperationException("Method is null");
    }
}