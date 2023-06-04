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
    public async Task TestBigInt()
    {
        var col = new PostgresBigInt(1, "MyTestCol", false);
        var result = await GetColFromTableDefinition(col);
        VerifyColumn(result, col);
    }

    [Fact]
    public async Task TestInt()
    {
        var col = new PostgresInt(1, "MyTestCol", false);
        var result = await GetColFromTableDefinition(col);
        VerifyColumn(result, col);
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