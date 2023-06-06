namespace APostgres.Test.DataReader;

public class PgDataReaderTestBase : PgTestBase
{
    protected string TestTableName { get; }
    protected readonly TableDefinition TableDefinition;
    protected readonly IADataReader ADataReader;
    protected FileHelper FileHelper;
    protected readonly IDataFileReaderFactory DataFileReaderFactory;

    protected PgDataReaderTestBase(ITestOutputHelper output, string tableName)
        : base(output)
    {
        TestTableName = tableName;
        TableDefinition = MssTestData.GetEmpty(TestTableName);
        FileHelper = new FileHelper(TableDefinition.Header.Name, DbServer.SqlServer);
        DataFileReaderFactory = new DataFileReaderFactory(FileHelper.FileSystem, TestLogger);
        ADataReader = new PgDataReader(PgContext, DataFileReaderFactory, TestLogger);
    }

    protected async Task<T> TestDataReader<T>(
        IColumn col,
        string fileData)
    {
        try
        {
            TableDefinition.Columns.Add(col);
            await PgDbHelper.Instance.DropTable(TestTableName);
            await PgDbHelper.Instance.CreateTable(TableDefinition);
            FileHelper.CreateDataFile(fileData);

            // Act
            await ADataReader.Read(FileHelper.DataFolder, TableDefinition);

            // Assert
            return await PgDbHelper.Instance.SelectScalar<T>(
                TestTableName, col.Name);
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(TestTableName);
        }
    }
}