namespace APostgres.Test.SchemaReader;

public class PgSchemaReaderBase : PgTestBase
{
    protected const string TableName = "MyTable";
    protected FileHelper FileHelper;
    protected ISchemaReader SchemaReader;

    public PgSchemaReaderBase(ITestOutputHelper output)
        : base(output)
    {
        FileHelper = new FileHelper();
        var typeConverter = new PgTypeMapper(
            new PgParser(),
            new ParseTree(new NodeFactory(), new SqlTypes()),
            new TokenizerFactory(new TokenFactory()),
            new PgColumnFactory(), 
            new MappingFactory());
        SchemaReader = new PgSchemaReader(typeConverter, FileHelper.FileSystem, TestLogger);
    }

    protected async Task<IColumn> GetColFromTableDefinition(IColumn col)
    {
        FileHelper.CreateSingleColMssSchemaFile(TableName, col);

        var tableDefinition = await SchemaReader.GetTableDefinitionAsync(FileHelper.DataFolder, TableName);

        tableDefinition.Should().NotBeNull();
        tableDefinition!.Header.Name.Should().Be(TableName);
        tableDefinition.Columns.Should().HaveCount(1);
        tableDefinition.Columns[0].Should().NotBeNull("because we should be able to cast to the correct type");
        return tableDefinition.Columns[0];
    }

    protected static void VerifyColumn(IColumn actual, IColumn expected)
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