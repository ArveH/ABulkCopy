namespace ABulkCopy.APostgres;

public interface IPgCmd
{
    Task CreateTable(TableDefinition tableDefinition, bool addIfNotExists = false);
    Task ExecuteNonQuery(string sqlString);
}