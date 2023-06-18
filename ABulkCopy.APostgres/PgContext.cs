using Microsoft.Extensions.Logging;

namespace ABulkCopy.APostgres;

public class PgContext : IPgContext
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly IConfiguration _config;
    private string? _connectionString;

    public PgContext(ILoggerFactory loggerFactory, IConfiguration config)
    {
        _loggerFactory = loggerFactory;
        _config = config;
        Rdbms = Rdbms.Pg;
    }

    public string ConnectionString
    {
        get
        {
            if (_connectionString != null) return _connectionString;

            _connectionString = _config.GetConnectionString(Constants.Config.DbKey);
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentException("Connection string is null or empty", nameof(ConnectionString));
            }

            return _connectionString;
        }
    }

    public Rdbms Rdbms { get; }

    private NpgsqlDataSource? _dataSource;
    public NpgsqlDataSource DataSource
    {
        get
        {
            if (_dataSource is null)
            {
                var dataSourceBuilder = new NpgsqlDataSourceBuilder(ConnectionString);
                dataSourceBuilder.UseLoggerFactory(_loggerFactory).EnableParameterLogging();
                _dataSource = dataSourceBuilder.Build();
            }
            return _dataSource;
        }
    }
}