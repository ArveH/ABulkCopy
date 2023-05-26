namespace ASqlServer.Test;

public abstract class MssDataWriterTestBase : MssTestBase
{
    public const string TestPath = @"C:\testfiles";
    protected string TestTableName { get; }
    protected readonly TableDefinition OriginalTableDefinition;
    protected readonly MockFileSystem MockFileSystem;
    protected readonly IDataWriter TestDataWriter;

    protected MssDataWriterTestBase(ITestOutputHelper output, string tableName)
        : base(output)
    {
        TestTableName = tableName;
        OriginalTableDefinition = MssTestData.GetEmpty(tableName);
        MockFileSystem = new MockFileSystem();
        MockFileSystem.AddDirectory(TestPath);
        TestDataWriter = new DataWriter(
            MssDbContext, 
            new TableReaderFactory(new SelectCreator(TestLogger), TestLogger),
            MockFileSystem, TestLogger);
    }

    protected async Task<string> ArrangeAndAct(IColumn col, object? value, SqlDbType? dbType = null)
    {
        // Arrange
        OriginalTableDefinition.Columns.Add(col);
        await MssDbHelper.Instance.DropTable(TestTableName);
        await MssDbHelper.Instance.CreateTable(OriginalTableDefinition);
        await MssDbHelper.Instance.InsertIntoSingleColumnTable(
            TestTableName, value, dbType);

        // Act
        try
        {
            await TestDataWriter.Write(
                OriginalTableDefinition,
                TestPath);
        }
        finally
        {
            await MssDbHelper.Instance.DropTable(TestTableName);
        }

        return await GetJsonText();
    }

    protected async Task TestWriteUsingType(IColumn col, object? value)
    {
        // Assert
        var jsonTxt = await ArrangeAndAct(col, value);
        jsonTxt.TrimEnd().Should().Be(value + ",");
    }

    protected async Task TestWrite(
        IColumn col,
        string actual,
        string? expected = null)
    {
        var jsonTxt = await ArrangeAndAct(col, actual);
        expected ??= actual;
        jsonTxt.TrimEnd().Should().Be($"{expected},");
    }

    protected static readonly string String10K = new ('a', 10000);
    protected static readonly string NString10K = new ('ﯵ', 10000);

    protected async Task<string> GetJsonText()
    {
        var fullPath = Path.Combine(TestPath, TestTableName + CommonConstants.DataSuffix);
        MockFileSystem.FileExists(fullPath).Should().BeTrue("because data file should exist");
        var jsonTxt = await MockFileSystem.File.ReadAllTextAsync(fullPath);
        return jsonTxt;
    }
}