namespace APostgres.Test.DataReader;

public class PgDataReaderTestBase : PgTestBase
{
    protected string TestTableName { get; }
    protected readonly TableDefinition TableDefinition;
    protected readonly IADataReader TestDataReader;
    protected FileHelper FileHelper;
    protected readonly IDataFileReaderFactory DataFileReaderFactory;

    protected PgDataReaderTestBase(ITestOutputHelper output, string tableName)
        : base(output)
    {
        TestTableName = tableName;
        TableDefinition = MssTestData.GetEmpty(TestTableName);
        FileHelper = new FileHelper(TableDefinition.Header.Name, DbServer.SqlServer);
        DataFileReaderFactory = new DataFileReaderFactory(FileHelper.FileSystem, TestLogger);
        TestDataReader = new PgDataReader(PgContext, DataFileReaderFactory, TestLogger);
    }


    protected async Task<T?> ArrangeAndAct<T>(
        IColumn col, T? fileValue)
    {
        // Arrange
        TableDefinition.Columns.Add(col);
        await PgDbHelper.Instance.DropTable(TestTableName);
        await PgDbHelper.Instance.CreateTable(TableDefinition);

        // Act
        try
        {
            await TestDataReader.Read(FileHelper.DataFolder, TableDefinition);
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(TestTableName);
        }

        return default;
    }
}