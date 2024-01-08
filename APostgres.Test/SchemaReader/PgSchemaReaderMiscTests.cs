namespace APostgres.Test.SchemaReader;

public class PgSchemaReaderMiscTests : PgSchemaReaderBase
{
    public PgSchemaReaderMiscTests(ITestOutputHelper output)
        : base(output)
    {
    }

    [Fact]
    public async Task ReadMssUniqueIdentifier()
    {
        var result = await GetColFromTableDefinition(new SqlServerUniqueIdentifier(1, "MyTestCol", false));
        VerifyColumn(result, new PostgresUuid(1, "MyTestCol", false));
    }

    [Fact]
    public async Task ReadMssBinary()
    {
        var result = await GetColFromTableDefinition(new SqlServerBinary(1, "MyTestCol", false, 2000));
        VerifyColumn(result, new PostgresByteA(1, "MyTestCol", false));
    }

    [Fact]
    public async Task ReadMssVarBinary()
    {
        var result = await GetColFromTableDefinition(new SqlServerBinary(1, "MyTestCol", false, -1));
        VerifyColumn(result, new PostgresByteA(1, "MyTestCol", false));
    }

    [Fact]
    public async Task ReadIdentityColumn()
    {
        var inputDefinition = MssTestData.GetEmpty(TableName);
        inputDefinition.Header.Identity = new Identity
        {
            Increment = 10,
            Seed = 100
        };
        var identityCol = new SqlServerBigInt(1, "agrtid", false)
        {
            Identity = inputDefinition.Header.Identity
        };
        inputDefinition.Columns.Add(identityCol);
        FileHelper.CreateSingleColMssSchemaFile(inputDefinition);

        var tableDefinition = await SchemaReader.GetTableDefinitionAsync(FileHelper.DataFolder, TableName);

        tableDefinition.Should().NotBeNull("because tableDefinition should not be null");
        tableDefinition!.Header.Name.Should().Be(TableName);
        tableDefinition.Columns.Should().HaveCount(1);
        tableDefinition.Columns[0].Should().NotBeNull("because we should have a column");
        tableDefinition.Columns[0].Identity.Should().BeEquivalentTo(identityCol.Identity);
    }
}