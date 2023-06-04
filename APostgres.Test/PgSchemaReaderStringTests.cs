namespace APostgres.Test;

public class PgSchemaReaderStringTests : PgSchemaReaderBase
{
    public PgSchemaReaderStringTests(ITestOutputHelper output) 
        : base(output)
    {
    }

    [Fact]
    public async Task ReadMssChar()
    {
        var result = await GetColFromTableDefinition(new SqlServerChar(1, "MyTestCol", false, 10));
        VerifyColumn(result, new PostgresChar(1, "MyTestCol", false, 10));
    }

    [Fact]
    public async Task ReadMssVarChar()
    {
        var result = await GetColFromTableDefinition(new SqlServerVarChar(1, "MyTestCol", false, 10));
        VerifyColumn(result, new PostgresVarChar(1, "MyTestCol", false, 10));
    }

    [Fact]
    public async Task ReadMssVarCharMax()
    {
        var result = await GetColFromTableDefinition(new SqlServerVarChar(1, "MyTestCol", false, -1));
        VerifyColumn(result, new PostgresText(1, "MyTestCol", false));
    }

    [Fact]
    public async Task ReadMssNChar()
    {
        var result = await GetColFromTableDefinition(new SqlServerNChar(1, "MyTestCol", false, 10));
        VerifyColumn(result, new PostgresChar(1, "MyTestCol", false, 10));
    }

    [Fact]
    public async Task ReadMssNVarChar()
    {
        var result = await GetColFromTableDefinition(new SqlServerNVarChar(1, "MyTestCol", false, 10));
        VerifyColumn(result, new PostgresVarChar(1, "MyTestCol", false, 10));
    }

    [Fact]
    public async Task ReadMssNVarCharMax()
    {
        var result = await GetColFromTableDefinition(new SqlServerNVarChar(1, "MyTestCol", false, -1));
        VerifyColumn(result, new PostgresText(1, "MyTestCol", false));
    }
}