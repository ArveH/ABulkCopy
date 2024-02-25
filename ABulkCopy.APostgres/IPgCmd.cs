﻿namespace ABulkCopy.APostgres;

public interface IPgCmd
{
    Task CreateTableAsync(
        TableDefinition tableDefinition, 
        CancellationToken ct, 
        bool addIfNotExists = false);
    Task ExecuteNonQueryAsync(
        string sqlString,
        CancellationToken ct);
    Task DropTableAsync(string tableName, CancellationToken ct);
    Task CreateIndexAsync(
        string tableName, 
        IndexDefinition indexDefinition, 
        CancellationToken ct);
    Task ResetIdentityAsync(
        string tableName, 
        string columnName, 
        CancellationToken ct);
    Task<object?> SelectScalarAsync(
        string sqlString,
        CancellationToken ct);
}