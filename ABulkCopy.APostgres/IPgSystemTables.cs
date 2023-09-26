﻿namespace ABulkCopy.APostgres;

public interface IPgSystemTables
{
    Task<PrimaryKey?> GetPrimaryKey(TableHeader tableHeader);
    Task<IEnumerable<ForeignKey>> GetForeignKeys(TableHeader tableHeader);
    Task<uint?> GetOid(char kind, string name);
}