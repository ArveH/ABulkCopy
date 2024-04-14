namespace APostgres.Tests.SchemaReader;

public class PgSchemaReaderNumbersTests : PgSchemaReaderBase
{
    public PgSchemaReaderNumbersTests(ITestOutputHelper output)
        : base(output)
    {
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
        VerifyColumn(result, new PostgresSmallInt(1, "MyTestCol", false));
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
}