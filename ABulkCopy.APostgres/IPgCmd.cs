namespace ABulkCopy.APostgres;

public interface IPgCmd
{
    Task CreateTable(TableDefinition tableDefinition, bool addIfNotExists = false);
    Task ExecuteNonQuery(string sqlString);
    Task DropTable(string tableName);
    Task CreateIndex(string tableName, IndexDefinition indexDefinition);
    Task ResetIdentity(string tableName, string columnName);
    Task<object?> SelectScalar(string sqlString);
}