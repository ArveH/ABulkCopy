using ABulkCopy.Common.Writer;

namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestChar : PgTestBase
{
    private const string ColName = "Col1";
    private static readonly string OutFolder = Path.Combine(".", "testout");

    private readonly FileHelper _fileHelper = new();
    private readonly Mock<IQueryBuilderFactory> _qbFactoryMock = new();
    private readonly IDataWriter _dataWriter;

    public TestChar(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
        _qbFactoryMock.Setup(f => f.GetQueryBuilder())
            .Returns(() => new QueryBuilder(
                new Identifier(dbFixture.Configuration, DbFixture.PgContext)));
        _fileHelper.FileSystem.AddDirectory(OutFolder);
        var selectCreator = new SelectCreator(GetIdentifier(), TestLogger);
        var tableReaderFactoryMock = new Mock<ITableReaderFactory>();
        tableReaderFactoryMock
            .Setup(f => f.GetTableReader(It.IsAny<IDbContext>()))
            .Returns((IDbContext dbContext) => new PgTableReader(selectCreator, TestLogger)
            {
                ConnectionString = dbContext.ConnectionString
            });
        _dataWriter = new DataWriter(
            DbFixture.PgContext,
            tableReaderFactoryMock.Object,
            _fileHelper.FileSystem,
            TestLogger);
    }

    [Fact]
    public async Task TestChar_When_BasicValue()
    {
        await TestCharRoundTrip(GetName(), 10, "Arve");
    }

    [Fact]
    public async Task TestChar_When_ValueIsExactLength()
    {
        await TestCharRoundTrip(GetName(), 10, "1234567890");
    }

    [Fact]
    public async Task TestChar_When_Length1()
    {
        await TestCharRoundTrip(GetName(), 1, "A");
    }

    [Fact]
    public async Task TestChar_When_Null()
    {
        await TestCharRoundTrip(GetName(), 10, null);
    }

    [Fact]
    public async Task TestChar_When_EmptyString()
    {
        await TestCharRoundTrip(GetName(), 10, "");
    }

    [Fact]
    public async Task TestChar_When_TrailingSpaces_Then_SpacesAreLost()
    {
        // char(n) pads with spaces, and trailing spaces are trimmed on copy out,
        // so "Arve  " is indistinguishable from "Arve" after the round trip
        await TestCharRoundTrip(GetName(), 10, "Arve  ",
            expectedFileValue: "Arve");
    }

    [Fact]
    public async Task TestChar_When_LeadingSpaces_Then_SpacesAreKept()
    {
        await TestCharRoundTrip(GetName(), 10, "  Arve");
    }

    [Fact]
    public async Task TestChar_When_SingleQuote()
    {
        await TestCharRoundTrip(GetName(), 15, $"Arve{Constants.QuoteChar}s vodka");
    }

    [Fact]
    public async Task TestChar_When_QuoteAtEnd()
    {
        await TestCharRoundTrip(GetName(), 10, $"123456789{Constants.QuoteChar}");
    }

    [Fact]
    public async Task TestChar_When_OnlyQuotes()
    {
        await TestCharRoundTrip(GetName(), 10, $"{Constants.QuoteChar}{Constants.QuoteChar}");
    }

    [Fact]
    public async Task TestChar_When_DoubleQuote()
    {
        await TestCharRoundTrip(GetName(), 10, "1234567\"89");
    }

    [Fact]
    public async Task TestChar_When_Comma()
    {
        await TestCharRoundTrip(GetName(), 10, "One, Two");
    }

    [Fact]
    public async Task TestChar_When_BackslashAtEnd()
    {
        await TestCharRoundTrip(GetName(), 10, "123456789\\");
    }

    [Fact]
    public async Task TestChar_When_Unicode()
    {
        await TestCharRoundTrip(GetName(), 10, "AﯵChar");
    }

    [Fact]
    public async Task TestChar_When_MultipleRows()
    {
        var tableName = GetName();
        SchemaTableTuple st = (DatabaseFixture.DefaultSchemaName, tableName);
        var col = new PostgresChar(1, ColName, true, 10);
        var values = new List<string?> { "Arve", null, "O'Hara", "" };
        var inputRows = values.Select(CreateRow).ToList();
        var tableDefinition = await CreateTableAndDataFileAsync(st, col, inputRows);
        try
        {
            // Act (copy in)
            var rowsRead = await CopyInAsync(tableDefinition);

            // Assert
            rowsRead.Should().Be(values.Count);
            (await DbFixture.GetRowCount(st)).Should().Be(values.Count);
            var dbValues = await DbFixture.SelectColumn<string>(st, ColName);
            dbValues.Should().Equal(
                "Arve".PadRight(10),
                null,
                "O'Hara".PadRight(10),
                "".PadRight(10));

            // Act (copy out)
            var outText = await CopyOutAsync(tableDefinition);

            // Assert
            outText.TrimEnd('\r', '\n').Should().Be(
                string.Join(Environment.NewLine, inputRows));
        }
        finally
        {
            await DbFixture.DropTable(st);
        }
    }

    private async Task TestCharRoundTrip(
        string tableName,
        int length,
        string? inputValue,
        string? expectedFileValue = null)
    {
        // Arrange
        SchemaTableTuple st = (DatabaseFixture.DefaultSchemaName, tableName);
        var col = new PostgresChar(1, ColName, true, length);
        var tableDefinition = await CreateTableAndDataFileAsync(
            st, col, [CreateRow(inputValue)]);
        try
        {
            // Act (copy in)
            var rowsRead = await CopyInAsync(tableDefinition);

            // Assert
            rowsRead.Should().Be(1);
            var dbValue = await DbFixture.SelectScalar<string>(st, col);
            if (inputValue == null)
            {
                dbValue.Should().BeNull();
            }
            else
            {
                dbValue.Should().Be(inputValue.PadRight(length));
            }

            // Act (copy out)
            var outText = await CopyOutAsync(tableDefinition);

            // Assert
            expectedFileValue ??= inputValue?.TrimEnd(' ');
            outText.TrimEnd('\r', '\n').Should().Be(CreateRow(expectedFileValue));
        }
        finally
        {
            await DbFixture.DropTable(st);
        }
    }

    private async Task<TableDefinition> CreateTableAndDataFileAsync(
        SchemaTableTuple st,
        IColumn col,
        List<string> fileRows)
    {
        var tableDefinition = PgTestData.GetEmpty(st);
        tableDefinition.Columns.Add(col);
        await DbFixture.DropTable(st);
        await DbFixture.CreateTable(tableDefinition);
        _fileHelper.CreateDataFile(st, fileRows);
        return tableDefinition;
    }

    private async Task<long> CopyInAsync(TableDefinition tableDefinition)
    {
        var dataReader = new PgDataReader(
            DbFixture.PgContext,
            _qbFactoryMock.Object,
            new DataFileReader(_fileHelper.FileSystem, TestLogger),
            TestLogger);
        return await dataReader.ReadAsync(
            _fileHelper.DataFolder,
            tableDefinition,
            CancellationToken.None,
            new InsertSettings());
    }

    private async Task<string> CopyOutAsync(TableDefinition tableDefinition)
    {
        await _dataWriter.WriteAsync(
            tableDefinition, OutFolder, CancellationToken.None);
        return await _fileHelper.FileSystem.GetJsonDataText(
            OutFolder, (tableDefinition.Header.Schema, tableDefinition.Header.Name));
    }

    private static string CreateRow(string? value)
    {
        if (value == null)
        {
            return Constants.ColumnSeparator;
        }

        return Constants.Quote
               + value.Replace(Constants.Quote, Constants.Quote + Constants.Quote)
               + Constants.Quote
               + Constants.ColumnSeparator;
    }
}
