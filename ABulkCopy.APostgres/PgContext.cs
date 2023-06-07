﻿using Microsoft.Extensions.Logging;

namespace ABulkCopy.APostgres;

public class PgContext : IPgContext
{
    private readonly ILoggerFactory _loggerFactory;

    public PgContext(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        DbServer = DbServer.Postgres;
    }

    public required string ConnectionString { get; init; }
    public DbServer DbServer { get; }

    private NpgsqlDataSource? _dataSource;
    public NpgsqlDataSource DataSource
    {
        get
        {
            if (_dataSource is null)
            {
                var dataSourceBuilder = new NpgsqlDataSourceBuilder(ConnectionString);
                dataSourceBuilder.UseLoggerFactory(_loggerFactory);
                _dataSource = dataSourceBuilder.Build();
            }
            return _dataSource;
        }
    }
}