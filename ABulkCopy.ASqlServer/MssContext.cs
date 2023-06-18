using Microsoft.Extensions.Configuration;

namespace ABulkCopy.ASqlServer;

public class MssContext : IDbContext
{
    private readonly IConfiguration _config;
    private string? _connectionString;

    public MssContext(IConfiguration config)
    {
        _config = config;
        Rdbms = Rdbms.Mss;
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
}