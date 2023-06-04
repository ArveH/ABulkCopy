namespace APostgres.Test;

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
}