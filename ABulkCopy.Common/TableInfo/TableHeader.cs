﻿namespace ABulkCopy.Common.TableInfo;

public class TableHeader
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Schema { get; set; }
    public required string Location { get; set; }
    public Identity? Identity { get; set; }
}