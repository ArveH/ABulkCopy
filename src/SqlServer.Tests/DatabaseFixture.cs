namespace SqlServer.Tests;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _mssContainer =
        new MsSqlBuilder().Build();
    private readonly IParseTree _parseTree = new ParseTree(new NodeFactory(), new SqlTypes());
    private readonly IPgParser _parser = new PgParser();
    private readonly ITokenizerFactory _tokenizerFactory = new TokenizerFactory(new TokenFactory());
    private IDbContext? _mssDbContext;
    private IConfiguration? _testConfiguration;

    public string MssConnectionString => _mssContainer.GetConnectionString();
    public string MssContainerId => $"{_mssContainer.Id}";

    public IDbContext MssDbContext
    {
        get => _mssDbContext ?? throw new ArgumentNullException(nameof(MssDbContext));
        set => _mssDbContext = value;
    }

    public IConfiguration TestConfiguration
    {
        get => _testConfiguration ?? throw new ArgumentNullException(nameof(TestConfiguration));
        set => _testConfiguration = value;
    }

    public async Task InitializeAsync()
    {
        await _mssContainer.StartAsync();
        TestConfiguration = new ConfigHelper().GetConfiguration(
            "5a78c96d-6df9-4362-ba25-4afceae69c52",
            new()
            {
                { Constants.Config.ConnectionString, MssConnectionString }
            });
        MssDbContext = new MssContext(TestConfiguration);
    }

    public async Task CreateTable(TableDefinition tableDefinition)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"create table [{tableDefinition.Header.Name}] (");
        var first = true;
        foreach (var column in tableDefinition.Columns)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                sb.AppendLine(",");
            }

            sb.Append($"    {column.Name} ");
            sb.Append(column.GetTypeClause());
            sb.Append(column.GetIdentityClause());
            if (column.HasDefault)
            {
                var tokenizer = _tokenizerFactory.GetTokenizer();
                tokenizer.Initialize(column.DefaultConstraint!.Definition);
                tokenizer.GetNext();
                var root = _parseTree.CreateExpression(tokenizer);
                sb.Append(" default ");
                sb.Append(_parser.Parse(tokenizer, root));
            }
            sb.Append(column.GetNullableClause());
        }
        sb.AppendLine(");");
        await ExecuteNonQuery(sb.ToString());
    }

    public async Task DropTable(string tableName)
    {
        await ExecuteNonQuery($"DROP TABLE IF EXISTS [{tableName}];");
    }

    public async Task InsertIntoSingleColumnTable(
        string tableName,
        object? value,
        SqlDbType? dbType = null)
    {
        await using var sqlConnection = new SqlConnection(MssConnectionString);
        await sqlConnection.OpenAsync();
        var sqlString = $"insert into [{tableName}] values (@Value);";
        await using var sqlCommand = new SqlCommand(sqlString, sqlConnection);
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
        await sqlCommand.ExecuteNonQueryAsync();
    }

    public async Task CreateIndex(string tableName, IndexDefinition indexDefinition)
    {
        var sb = new StringBuilder();
        sb.Append("create ");
        if (indexDefinition.Header.IsUnique)
        {
            sb.Append("unique ");
        }

        if (indexDefinition.Header.Type == IndexType.Clustered)
        {
            sb.Append("clustered ");
        }

        sb.Append("index ");
        sb.Append($"[{indexDefinition.Header.Name}] on ");
        sb.Append($"[{tableName}] (");
        var first = true;
        foreach (var indexCol in indexDefinition.Columns)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                sb.AppendLine(",");
            }

            sb.Append($"    {indexCol.Name} ");
            if (indexCol.Direction == Direction.Descending)
            {
                sb.Append("desc");
            }
        }
        sb.Append(")");

        await ExecuteNonQuery(sb.ToString());
    }

    public async Task DropIndex(string tableName, string indexName)
    {
        var sqlString = $"if exists (select name from sys.indexes where object_id=object_id('{tableName}') and name = '{indexName}' drop index [{tableName}].[{indexName}];";
        await ExecuteNonQuery(sqlString);
    }

    public async Task ExecuteNonQuery(string sqlString)
    {
        await using var sqlConnection = new SqlConnection(MssConnectionString);
        await sqlConnection.OpenAsync();
        await using var sqlCommand = new SqlCommand(sqlString, sqlConnection);
        await sqlCommand.ExecuteNonQueryAsync();
    }

    public async Task DisposeAsync()
    {
        await _mssContainer.DisposeAsync();
    }
}