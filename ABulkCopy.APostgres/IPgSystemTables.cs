namespace ABulkCopy.APostgres;

public interface IPgSystemTables
{
    Task<PrimaryKey?> GetPrimaryKey(TableHeader tableHeader);
}