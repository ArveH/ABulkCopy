using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ABulkCopy.APostgres;

public class PgContext : IPgContext
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly IConfiguration _config;
    private string? _connectionString;

    public PgContext(ILoggerFactory? loggerFactory, IConfiguration config)
    {
        _loggerFactory = loggerFactory ?? new NullLoggerFactory();
        _config = config;
        Rdbms = Rdbms.Pg;
    }

    public string ConnectionString
    {
        get
        {
            if (_connectionString != null) return _connectionString;

            _connectionString = _config.GetConnectionString(Constants.Config.PgConnectionString);
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentException("Connection string is null or empty", nameof(ConnectionString));
            }

            return _connectionString;
        }
    }

    public Rdbms Rdbms { get; }
    public int MaxIdentifierLength => 63;

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

    public void Dispose()
    {
        _dataSource?.Dispose();
        _dataSource = null;
    }
}