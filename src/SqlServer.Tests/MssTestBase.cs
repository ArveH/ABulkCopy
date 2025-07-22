using System.Data.SqlClient;

namespace SqlServer.Tests;

public abstract class MssTestBase
{
    protected readonly DatabaseFixture DbFixture;
    protected readonly ILogger TestLogger;
    protected readonly Microsoft.Extensions.Logging.ILoggerFactory TestLoggerFactory;
    protected readonly IMssSystemTables MssSystemTables;

    protected MssTestBase(DatabaseFixture dbFixture, ITestOutputHelper output)
    {
        DbFixture = dbFixture;
        const string outputTemplate =
            "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message}    {Timestamp:yyyy-MM-dd }{Properties}{NewLine}{Exception}{NewLine}";

        TestLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.Console(outputTemplate: outputTemplate)
            .WriteTo.TestOutput(output)
            .CreateLogger();
        TestLoggerFactory = new Microsoft.Extensions.Logging.LoggerFactory().AddSerilog(TestLogger);
        
        MssSystemTables = CreateMssSystemTables();
    }

    private IMssSystemTables CreateMssSystemTables()
    {
        var connectionString = DbFixture.TestConfiguration.GetConnectionString(Constants.Config.MssConnectionString);
        connectionString.Should()
            .NotBeNullOrWhiteSpace("because the connection string should be set");
        IMssColumnFactory colFactory = new MssColumnFactory();
        IMssSystemTables systemTables = new MssSystemTables(
            DbFixture.MssRawCommand,
            colFactory, 
            TestLogger);
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
        return methodName.Length > 44 ? methodName[4..44] : methodName;
    }

    public async Task CreateTableAsync(TableDefinition tableDefinition)
    {
        await DbFixture.MssCmd.CreateTableAsync(tableDefinition, CancellationToken.None);
    }

    public async Task DropTableAsync(string tableName)
    {
        await DropTableAsync("dbo", tableName);
    }

    public async Task DropTableAsync(string schema, string tableName)
    {
        await DbFixture.MssCmd.DropTableAsync((schema, tableName), CancellationToken.None);
    }
    
    public async Task InsertIntoSingleColumnTableAsync(
        string tableName,
        object? value,
        SqlDbType? dbType = null,
        CancellationToken ct = default)
    {
        var sqlString = $"insert into [{tableName}] values (@Value);";
        await using var sqlCommand = new SqlCommand(sqlString);
        if (dbType == null)
        {
            sqlCommand.Parameters.AddWithValue("@Value", value ?? DBNull.Value);
        }
        else
        {
            // Note: Since dbType is nullable, we have to cast to non-nullable
            // otherwise we get the wrong SqlParameter constructor
            // and dbType is treated as the value instead of the SqlDbType
            var sqlParameter = new SqlParameter("@Value", (SqlDbType)dbType);
            sqlParameter.Value = value ?? DBNull.Value;
            sqlCommand.Parameters.Add(sqlParameter);
        }
        await DbFixture.MssRawCommand.ExecuteNonQueryAsync(
            sqlCommand, ct).ConfigureAwait(false);
    }

    public async Task CreateIndexAsync(string tableName, IndexDefinition indexDefinition)
    {
        await DbFixture.MssCmd.CreateIndexAsync(("dbo", tableName), indexDefinition, CancellationToken.None);
    }

    public async Task DropIndexAsync(string tableName, string indexName)
    {
        await DbFixture.MssCmd.DropIndexAsync(("dbo", tableName), indexName, CancellationToken.None);
    }

    public async Task ExecuteNonQueryAsync(string sqlString)
    {
        await DbFixture.MssRawCommand.ExecuteNonQueryAsync(sqlString, CancellationToken.None);
    }

    public async Task<T?> ExecuteScalarAsync<T>(string sqlString)
    {
        return (T?)await DbFixture.MssRawCommand.ExecuteScalarAsync(sqlString, CancellationToken.None);
    }
}