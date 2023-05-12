﻿namespace ASqlServer;

public interface IASqlCommand
{
    string ConnectionString { get; init; }
    Task<IEnumerable<string>> GetTableNames(string searchString);
    Task<TableDefinition?> GetTableInfo(string tableName);
    Task<IEnumerable<ColumnDefinition>> GetColumnInfo(TableDefinition tableDef);
    Task<PrimaryKey?> GetPrimaryKey(TableDefinition tableDef);
    Task<IEnumerable<ForeignKey>> GetForeignKeys(TableDefinition tableDef);
}