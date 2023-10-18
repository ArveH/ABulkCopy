namespace Testing.Shared.SqlServer;

public class MssDbHelper
{
    private static readonly Lazy<MssDbHelper> LazyInstance =
        new(() => new MssDbHelper());

    private readonly string? _connectionString;

    private MssDbHelper()
    {
        var configuration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");
        _connectionString = configuration.Check(TestConstants.Config.ConnectionString);
    }

    public static MssDbHelper Instance => LazyInstance.Value;
    public string ConnectionString => _connectionString ?? throw new ArgumentNullException(nameof(ConnectionString));

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
                // TODO: Inject factories or otherwise remove all 'new' statements
                ITokenizer tokenizer = new Tokenizer(new TokenFactory());
                tokenizer.Initialize(column.DefaultConstraint!.Definition);
                tokenizer.GetNext();
                IParseTree parseTree = new ParseTree(new AParser.Tree.NodeFactory(), new SqlTypes());
                var root = parseTree.CreateExpression(tokenizer);
                sb.Append(" default ");
                sb.Append(new PgParser().Parse(tokenizer, root));
            }
            sb.Append(column.GetNullableClause());
        }
        sb.AppendLine(");");
        await ExecuteNonQuery(sb.ToString());
    }

    public async Task DropTable(string tableName)
    {
        var sqlString = $"if exists (select name from sys.objects where type='U' and name='{tableName}') drop table [{tableName}];";
        await ExecuteNonQuery(sqlString);
    }

    public async Task InsertIntoSingleColumnTable(
        string tableName,
        object? value,
        SqlDbType? dbType=null)
    {
        await using var sqlConnection = new SqlConnection(ConnectionString);
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
        await using var sqlConnection = new SqlConnection(ConnectionString);
        await sqlConnection.OpenAsync();
        await using var sqlCommand = new SqlCommand(sqlString, sqlConnection);
        await sqlCommand.ExecuteNonQueryAsync();
    }
}