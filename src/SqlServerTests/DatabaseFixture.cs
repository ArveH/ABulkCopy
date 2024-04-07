using AParser.Parsers.Pg;
using AParser.Tree;
using AParser;
using System.Data.SqlClient;
using System.Text;

namespace SqlServerTests;

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
        await ExecuteNonQuery(
            "CREATE TABLE [dbo].[AllTypes](\r\n\t[Id] [bigint] IDENTITY(1,1) NOT NULL,\r\n\t[ExactNumBigInt] [bigint] NOT NULL,\r\n\t[ExactNumInt] [int] NOT NULL,\r\n\t[ExactNumSmallInt] [smallint] NOT NULL,\r\n\t[ExactNumTinyInt] [tinyint] NOT NULL,\r\n\t[ExactNumBit] [bit] NOT NULL,\r\n\t[ExactNumMoney] [money] NOT NULL,\r\n\t[ExactNumSmallMoney] [smallmoney] NOT NULL,\r\n\t[ExactNumDecimal] [decimal](28, 3) NOT NULL,\r\n\t[ExactNumNumeric] [numeric](28, 3) NOT NULL,\r\n\t[ApproxNumFloat] [float] NOT NULL,\r\n\t[ApproxNumReal] [real] NOT NULL,\r\n\t[DTDate] [date] NOT NULL,\r\n\t[DTDateTime] [datetime] NOT NULL,\r\n\t[DTDateTime2] [datetime2](7) NOT NULL,\r\n\t[DTSmallDateTime] [smalldatetime] NOT NULL,\r\n\t[DTDateTimeOffset] [datetimeoffset](7) NOT NULL,\r\n\t[DTTime] [time](7) NOT NULL,\r\n\t[CharStrChar20] [char](20) NULL,\r\n\t[CharStrVarchar20] [varchar](20) NULL,\r\n\t[CharStrVarchar10K] [varchar](max) NULL,\r\n\t[CharStrNChar20] [nchar](20) NULL,\r\n\t[CharStrNVarchar20] [nvarchar](20) NULL,\r\n\t[CharStrNVarchar10K] [nvarchar](max) NULL,\r\n\t[BinBinary5K] [binary](5000) NULL,\r\n\t[BinVarbinary10K] [varbinary](max) NULL,\r\n\t[OtherGuid] [uniqueidentifier] NOT NULL,\r\n\t[OtherXml] [xml] NULL,\r\n CONSTRAINT [PK_AllTypes] PRIMARY KEY CLUSTERED \r\n(\r\n\t[Id] ASC\r\n))");
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
        var sqlString = $"if exists (select name from sys.objects where type='U' and name='{tableName}') drop table [{tableName}];";
        await ExecuteNonQuery(sqlString);
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