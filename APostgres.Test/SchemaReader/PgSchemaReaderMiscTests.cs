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
}