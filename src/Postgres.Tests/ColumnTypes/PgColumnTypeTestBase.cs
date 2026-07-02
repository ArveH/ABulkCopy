using ABulkCopy.Common.Writer;

namespace Postgres.Tests.ColumnTypes;

public abstract class PgColumnTypeTestBase : PgTestBase
{
    protected const string ColName = "Col1";
    protected static readonly string OutFolder = Path.Combine(".", "testout");

    protected readonly FileHelper FileHelper = new();
    protected readonly IDataWriter DataWriter;

    private readonly Mock<IQueryBuilderFactory> _qbFactoryMock = new();

    protected PgColumnTypeTestBase(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
        _qbFactoryMock.Setup(f => f.GetQueryBuilder())
            .Returns(() => new QueryBuilder(
                new Identifier(dbFixture.Configuration, DbFixture.PgContext)));
        FileHelper.FileSystem.AddDirectory(OutFolder);
        var selectCreator = new SelectCreator(GetIdentifier(), TestLogger);
        var tableReaderFactoryMock = new Mock<ITableReaderFactory>();
        tableReaderFactoryMock
            .Setup(f => f.GetTableReader(It.IsAny<IDbContext>()))
            .Returns((IDbContext dbContext) => new PgTableReader(selectCreator, TestLogger)
            {
                ConnectionString = dbContext.ConnectionString
            });
        DataWriter = new DataWriter(
            DbFixture.PgContext,
            tableReaderFactoryMock.Object,
            FileHelper.FileSystem,
            TestLogger);
    }

    /// <summary>
    /// Full end-to-end round trip for a single column value:
    /// data file -> copy in (PgDataReader) -> assert db value ->
    /// copy out (DataWriter + PgTableReader) -> assert file content.
    /// </summary>
    /// <param name="inputCell">The column value exactly as it appears in the
    /// data file (quoted/escaped for strings, bare for numbers). Null means
    /// a null value (empty cell).</param>
    /// <param name="expectedDbValue">Expected value in the database after copy in.</param>
    /// <param name="expectedOutputCell">Expected cell content after copy out.
    /// Defaults to <paramref name="inputCell"/>.</param>
    protected async Task TestColumnRoundTrip<T>(
        string tableName,
        IColumn col,
        string? inputCell,
        T? expectedDbValue,
        string? expectedOutputCell = null)
    {
        // Arrange
        SchemaTableTuple st = (DatabaseFixture.DefaultSchemaName, tableName);
        var tableDefinition = await CreateTableAndDataFileAsync(
            st, col, [ToRow(inputCell)]);
        try
        {
            // Act (copy in)
            var rowsRead = await CopyInAsync(tableDefinition);

            // Assert
            rowsRead.Should().Be(1);
            var dbValue = await DbFixture.SelectScalar<T>(st, col);
            if (expectedDbValue == null)
            {
                dbValue.Should().BeNull();
            }
            else
            {
                dbValue.Should().Be(expectedDbValue);
            }

            // Act (copy out)
            var outText = await CopyOutAsync(tableDefinition);

            // Assert
            outText.TrimEnd('\r', '\n').Should().Be(
                ToRow(expectedOutputCell ?? inputCell));
        }
        finally
        {
            await DbFixture.DropTable(st);
        }
    }

    protected async Task<TableDefinition> CreateTableAndDataFileAsync(
        SchemaTableTuple st,
        IColumn col,
        List<string> fileRows)
    {
        var tableDefinition = PgTestData.GetEmpty(st);
        tableDefinition.Columns.Add(col);
        await DbFixture.DropTable(st);
        await DbFixture.CreateTable(tableDefinition);
        FileHelper.CreateDataFile(st, fileRows);
        return tableDefinition;
    }

    protected async Task<long> CopyInAsync(TableDefinition tableDefinition)
    {
        var dataReader = new PgDataReader(
            DbFixture.PgContext,
            _qbFactoryMock.Object,
            new DataFileReader(FileHelper.FileSystem, TestLogger),
            TestLogger);
        return await dataReader.ReadAsync(
            FileHelper.DataFolder,
            tableDefinition,
            CancellationToken.None,
            new InsertSettings());
    }

    protected async Task<string> CopyOutAsync(TableDefinition tableDefinition)
    {
        await DataWriter.WriteAsync(
            tableDefinition, OutFolder, CancellationToken.None);
        return await FileHelper.FileSystem.GetJsonDataText(
            OutFolder, (tableDefinition.Header.Schema, tableDefinition.Header.Name));
    }

    protected static string ToRow(string? cell)
    {
        return (cell ?? "") + Constants.ColumnSeparator;
    }

    protected static string QuoteCell(string value)
    {
        return Constants.Quote
               + value.Replace(Constants.Quote, Constants.Quote + Constants.Quote)
               + Constants.Quote;
    }
}
