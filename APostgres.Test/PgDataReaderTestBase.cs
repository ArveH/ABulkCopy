namespace APostgres.Test;

public class PgDataReaderTestBase : PgTestBase
{
    public const string TestPath = @"C:\testfiles";
    protected string TestTableName { get; }
    protected readonly TableDefinition OriginalTableDefinition;
    protected readonly MockFileSystem MockFileSystem;
    protected readonly IADataReader TestDataReader;
    protected FileHelper FileHelper;

    protected PgDataReaderTestBase(ITestOutputHelper output, string tableName)
        : base(output)
    {
        TestTableName = tableName;
        OriginalTableDefinition = MssTestData.GetEmpty(TestTableName);
        MockFileSystem = new MockFileSystem();
        MockFileSystem.AddDirectory(TestPath);
        TestDataReader = new ADataReader();
        FileHelper = new FileHelper(TestTableName, DbServer.Postgres);
    }


    protected async Task<T?> ArrangeAndAct<T>(
        IColumn col, T? fileValue)
    {
        // Arrange
        OriginalTableDefinition.Columns.Add(col);
        await PgDbHelper.Instance.DropTable(TestTableName);
        await PgDbHelper.Instance.CreateTable(OriginalTableDefinition);

        // Act
        try
        {
            await TestDataReader.Read(
                OriginalTableDefinition,
                TestPath);
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(TestTableName);
        }

        return default;
    }
}