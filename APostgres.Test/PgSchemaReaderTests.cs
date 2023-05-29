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
    public async Task SchemaReaderForNumbersTests()
    {
        // Arrange
        var col = new PostgresBigInt(1, "MyTestCol", false)
        {
            Precision = 19,
            Scale = 0,
            Collation = null
        };
        var fileHelper = new SchemaFileHelper(TableName);
        fileHelper.CreateSingleColFile(TestPath, col);
        ISchemaReader schemaReader = new PgSchemaReader(fileHelper.FileSystem, TestLogger);

        // Act
        var tableDefinition = await schemaReader.GetTableDefinition(TestPath, TableName);

        // Assert
        tableDefinition.Should().NotBeNull();
        tableDefinition!.Header.Name.Should().Be(TableName);
        tableDefinition.Columns.Should().HaveCount(1);
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