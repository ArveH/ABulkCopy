namespace ASqlServer;

public interface IMssCommand
{
    string ConnectionString { get; init; }
    Task<IEnumerable<string>> GetTableNames(string searchString);
    Task<TableHeader?> GetTableHeader(string tableName);
    Task<IEnumerable<ColumnDefinition>> GetColumnInfo(TableHeader tableHeader);
    Task<PrimaryKey?> GetPrimaryKey(TableHeader tableHeader);
    Task<IEnumerable<ForeignKey>> GetForeignKeys(TableHeader tableHeader);
}