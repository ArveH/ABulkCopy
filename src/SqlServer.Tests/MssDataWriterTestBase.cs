using Testing.Shared;

namespace SqlServer.Tests;

public abstract class MssDataWriterTestBase : MssTestBase
{
    public const string TestPath = @"C:\testfiles";
    protected string TestTableName { get; }
    protected readonly TableDefinition OriginalTableDefinition;
    protected readonly MockFileSystem MockFileSystem;
    protected readonly IDataWriter TestDataWriter;

    protected MssDataWriterTestBase(DatabaseFixture dbFixture, ITestOutputHelper output, string tableName)
        : base(dbFixture, output)
    {
        TestTableName = tableName;
        OriginalTableDefinition = MssTestData.GetEmpty(("dbo", tableName));
        MockFileSystem = new MockFileSystem();
        MockFileSystem.AddDirectory(TestPath);
        TestDataWriter = new DataWriter(
            DbFixture.MssDbContext,
            new TableReaderFactoryForTest(new SelectCreator(TestLogger), TestLogger),
            MockFileSystem, TestLogger);
    }

    protected async Task<string> ArrangeAndAct(IColumn col, object? value, SqlDbType? dbType = null)
    {
        // Arrange
        OriginalTableDefinition.Columns.Add(col);
        await DropTable(TestTableName);
        await CreateTable(OriginalTableDefinition);
        await InsertIntoSingleColumnTable(
            TestTableName, value, dbType);
        var cts = new CancellationTokenSource();

        // Act
        try
        {
            await TestDataWriter.WriteAsync(
                OriginalTableDefinition,
                TestPath,
                cts.Token);
        }
        finally
        {
            await DropTable(TestTableName);
        }

        return await MockFileSystem.GetJsonDataText(TestPath, ("dbo", TestTableName));
    }

    protected async Task TestWriteUsingType(IColumn col, object? value)
    {
        // Assert
        var jsonTxt = await ArrangeAndAct(col, value);
        jsonTxt.TrimEnd().Should().Be(value + Constants.ColumnSeparator);
    }

    protected async Task TestWrite(
        IColumn col,
        string actual,
        string? expected = null)
    {
        var jsonTxt = await ArrangeAndAct(col, actual);
        expected ??= actual;
        jsonTxt.TrimEnd().Should().Be($"{expected}{Constants.ColumnSeparatorChar}");
    }

    protected static readonly string String10K = new('a', 10000);
    protected static readonly string NString10K = new('ﯵ', 10000);
}