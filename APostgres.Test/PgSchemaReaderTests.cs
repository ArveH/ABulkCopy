namespace APostgres.Test;

public class PgSchemaReaderTests : PgTestBase
{
    private const string TableName = "MyTabl";
    private const string TestPath = ".\\testpath";

    public PgSchemaReaderTests(ITestOutputHelper output) 
        : base(output)
    {
    }

    [Fact]
    public async Task TestBigInt()
    {
        // Arrange
        var col = new PostgresBigInt(1, "MyTestCol", false);
        var fileHelper = new SchemaFileHelper(TableName);
        fileHelper.CreateSingleColMssSchemaFile(TestPath, col);
        ISchemaReader schemaReader = new PgSchemaReader(fileHelper.FileSystem, TestLogger);

        // Act
        var tableDefinition = await schemaReader.GetTableDefinition(TestPath, TableName);

        // Assert
        tableDefinition.Should().NotBeNull();
        tableDefinition!.Header.Name.Should().Be(TableName);
        tableDefinition.Columns.Should().HaveCount(1);
        var colType = tableDefinition.Columns[0] as PostgresBigInt;
        colType.Should().NotBeNull("because we should be able to cast to the correct type");
        VerifyColumn(tableDefinition.Columns[0], col);
    }

    [Fact]
    public async Task TestInt()
    {
        // Arrange
        var col = new PostgresInt(1, "MyTestCol", false);
        var fileHelper = new SchemaFileHelper(TableName);
        fileHelper.CreateSingleColMssSchemaFile(TestPath, col);
        ISchemaReader schemaReader = new PgSchemaReader(fileHelper.FileSystem, TestLogger);

        // Act
        var tableDefinition = await schemaReader.GetTableDefinition(TestPath, TableName);

        // Assert
        tableDefinition.Should().NotBeNull();
        tableDefinition!.Columns.Should().HaveCount(1);
        var colType = tableDefinition.Columns[0] as PostgresInt;
        colType.Should().NotBeNull("because we should be able to cast to the correct type");
        VerifyColumn(tableDefinition.Columns[0], col);
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