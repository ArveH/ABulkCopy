using ABulkCopy.ASqlServer.Column.ColumnTypes;

namespace APostgres.Test;

public class PgSchemaReaderTests : PgTestBase
{
    private const string TableName = "MyTable";
    private const string TestPath = ".\\testpath";
    private readonly SchemaFileHelper _fileHelper;
    private readonly ISchemaReader _schemaReader;

    public PgSchemaReaderTests(ITestOutputHelper output) 
        : base(output)
    {
        _fileHelper = new SchemaFileHelper(TableName);
        var columnFactory = new PgColumnFactory();
        var mappingFactory = new MappingFactory();
        var typeConverter = new PgTypeMapper(columnFactory, mappingFactory);
        _schemaReader = new PgSchemaReader(typeConverter, _fileHelper.FileSystem, TestLogger);
    }

    private async Task<IColumn> GetColFromTableDefinition(IColumn col)
    {
        _fileHelper.CreateSingleColMssSchemaFile(TestPath, col);

        var tableDefinition = await _schemaReader.GetTableDefinition(TestPath, TableName);

        tableDefinition.Should().NotBeNull();
        tableDefinition!.Header.Name.Should().Be(TableName);
        tableDefinition.Columns.Should().HaveCount(1);
        tableDefinition.Columns[0].Should().NotBeNull("because we should be able to cast to the correct type");
        return tableDefinition.Columns[0];
    }

    [Fact]
    public async Task ReadMssBigInt()
    {
        var result = await GetColFromTableDefinition(new SqlServerBigInt(1, "MyTestCol", false));
        VerifyColumn(result, new PostgresBigInt(1, "MyTestCol", false));
    }

    [Fact]
    public async Task TestInt()
    {
        var result = await GetColFromTableDefinition(new SqlServerInt(1, "MyTestCol", false));
        VerifyColumn(result, new PostgresInt(1, "MyTestCol", false));
    }

    [Fact]
    public async Task TestSmallInt()
    {
        var result = await GetColFromTableDefinition(new SqlServerSmallInt(1, "MyTestCol", false));
        VerifyColumn(result, new PostgresSmallInt(1, "MyTestCol", false));
    }

    [Fact]
    public async Task TestTinyInt()
    {
        var result = await GetColFromTableDefinition(new SqlServerTinyInt(1, "MyTestCol", false));
        VerifyColumn(result, new PostgresSmallInt(1, "MyTestCol", false));
    }

    [Fact]
    public async Task TestBit()
    {
        var result = await GetColFromTableDefinition(new SqlServerBit(1, "MyTestCol", false));
        VerifyColumn(result, new PostgresBoolean(1, "MyTestCol", false));
    }

    [Fact]
    public async Task TestDecimal()
    {
        var result = await GetColFromTableDefinition(new SqlServerDecimal(1, "MyTestCol", false, 32, 4));
        VerifyColumn(result, new PostgresDecimal(1, "MyTestCol", false, 32, 4));
    }

    [Fact]
    public async Task TestMoney()
    {
        var result = await GetColFromTableDefinition(new SqlServerMoney(1, "MyTestCol", false));
        VerifyColumn(result, new PostgresMoney(1, "MyTestCol", false));
    }

    [Fact]
    public async Task TestReal()
    {
        var result = await GetColFromTableDefinition(new SqlServerFloat(1, "MyTestCol", false, 13));
        VerifyColumn(result, new PostgresReal(1, "MyTestCol", false));
    }

    [Fact]
    public async Task TestFloat()
    {
        var result = await GetColFromTableDefinition(new SqlServerFloat(1, "MyTestCol", false, 32));
        VerifyColumn(result, new PostgresDoublePrecision(1, "MyTestCol", false));
    }

    private static void VerifyColumn(IColumn actual, IColumn expected)
    {
        actual.Name.Should().Be(expected.Name);
        actual.Type.Should().Be(expected.Type);
        actual.IsNullable.Should().Be(expected.IsNullable);
        actual.Identity.Should().BeNull();
        actual.ComputedDefinition.Should().BeNull();
        actual.Length.Should().Be(expected.Length);
        actual.Precision.Should().Be(expected.Precision);
        actual.Scale.Should().Be(expected.Scale);
        actual.DefaultConstraint.Should().BeNull();
        actual.Collation.Should().Be(expected.Collation);
    }
}