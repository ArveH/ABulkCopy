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

    public async Task CreateTableAsync(TableDefinition tableDefinition)
    {
        await DbFixture.DbHelper.CreateTableAsync(tableDefinition);
    }

    public async Task DropTableAsync(string tableName)
    {
        await DropTableAsync("dbo", tableName);
    }

    public async Task DropTableAsync(string schema, string tableName)
    {
        await DbFixture.DbHelper.DropTableAsync((schema, tableName));
    }

    public async Task InsertIntoSingleColumnTableAsync(
        string tableName,
        object? value,
        SqlDbType? dbType = null)
    {
        await DbFixture.DbHelper.InsertIntoSingleColumnTableAsync(tableName, value, dbType);
    }

    public async Task CreateIndexAsync(string tableName, IndexDefinition indexDefinition)
    {
        await DbFixture.DbHelper.CreateIndexAsync(tableName, indexDefinition);
    }

    public async Task DropIndexAsync(string tableName, string indexName)
    {
        await DbFixture.DbHelper.DropIndexAsync(tableName, indexName);
    }

    public async Task ExecuteNonQueryAsync(string sqlString)
    {
        await DbFixture.DbHelper.ExecuteNonQueryAsync(sqlString, CancellationToken.None);
    }

    public async Task<T?> ExecuteScalarAsync<T>(string sqlString)
    {
        return await DbFixture.DbHelper.ExecuteScalarAsync<T>(sqlString);
    }
}