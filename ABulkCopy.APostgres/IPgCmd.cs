namespace ABulkCopy.APostgres;

public interface IPgCmd
{
    Task CreateTableAsync(TableDefinition tableDefinition, bool addIfNotExists = false);
    Task ExecuteNonQueryAsync(string sqlString);
    Task DropTableAsync(string tableName);
    Task CreateIndexAsync(string tableName, IndexDefinition indexDefinition);
    Task ResetIdentityAsync(string tableName, string columnName);
    Task<object?> SelectScalarAsync(string sqlString);
}