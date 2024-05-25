namespace SqlServer.Tests;

public abstract class MssTestBase
{
    protected readonly DatabaseFixture DbFixture;
    protected readonly ILogger TestLogger;
    protected readonly IMssSystemTables MssSystemTables;

    protected MssTestBase(DatabaseFixture dbFixture, ITestOutputHelper output)
    {
        DbFixture = dbFixture;
        TestLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        MssSystemTables = CreateMssSystemTables();
    }

    private IMssSystemTables CreateMssSystemTables()
    {
        var connectionString = DbFixture.TestConfiguration.GetConnectionString(Constants.Config.MssConnectionString);
        connectionString.Should()
            .NotBeNullOrWhiteSpace("because the connection string should be set");
        IMssColumnFactory colFactory = new MssColumnFactory();
        IMssSystemTables systemTables = new MssSystemTables(
            DbFixture.MssDbContext,
            colFactory, TestLogger);
        return systemTables;
    }

    protected string GetName()
    {
        var st = new StackTrace();
        // Frames:
        //   0: GetName
        //   1: MoveNext
        //   2: Start
        //   3: <Should be the name of the test method>
        var sf = st.GetFrame(3);
        if (sf == null)
        {
            throw new InvalidOperationException("Stack Frame is null");
        }

        var methodName = sf.GetMethod()?.Name ?? throw new InvalidOperationException("Method is null");
        return methodName.Length > 34 ? methodName[4..34] : methodName;
    }

    public async Task CreateTable(TableDefinition tableDefinition)
    {
        await DbFixture.DbHelper.CreateTable(tableDefinition);
    }

    public async Task DropTable(string tableName)
    {
        await DropTable("dbo", tableName);
    }

    public async Task DropTable(string schema, string tableName)
    {
        await DbFixture.DbHelper.DropTable((schema, tableName));
    }

    public async Task InsertIntoSingleColumnTable(
        string tableName,
        object? value,
        SqlDbType? dbType = null)
    {
        await DbFixture.DbHelper.InsertIntoSingleColumnTable(tableName, value, dbType);
    }

    public async Task CreateIndex(string tableName, IndexDefinition indexDefinition)
    {
        await DbFixture.DbHelper.CreateIndex(tableName, indexDefinition);
    }

    public async Task DropIndex(string tableName, string indexName)
    {
        await DbFixture.DbHelper.DropIndex(tableName, indexName);
    }

    public async Task ExecuteNonQuery(string sqlString)
    {
        await DbFixture.DbHelper.ExecuteNonQuery(sqlString);
    }

    public async Task<T?> ExecuteScalarAsync<T>(string sqlString)
    {
        return await DbFixture.DbHelper.ExecuteScalarAsync<T>(sqlString);
    }
}