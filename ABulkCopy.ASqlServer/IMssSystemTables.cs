﻿namespace ABulkCopy.ASqlServer;

public interface IMssSystemTables
{
    Task<IEnumerable<string>> GetTableNames(string searchString);
    Task<TableHeader?> GetTableHeader(string tableName);
    Task<IEnumerable<IColumn>> GetTableColumnInfo(TableHeader tableHeader);
    Task<PrimaryKey?> GetPrimaryKey(TableHeader tableHeader);
    Task<IEnumerable<ForeignKey>> GetForeignKeys(TableHeader tableHeader);
    Task<IEnumerable<IndexDefinition>> GetIndexes(TableHeader tableHeader);
}