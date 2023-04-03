﻿namespace ABulkCopy.Common.TableInfo;

public class TableDefinition
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Schema { get; set; }
    public required string Location { get; set; }

    public List<ColumnDefinition> Columns { get; set; } = new();
    public List<IdentityCol> IdentityColumns { get; set; } = new();
    public PrimaryKey? PrimaryKey { get; set; }
    public List<ForeignKey> ForeignKeys { get; set; } = new();
}