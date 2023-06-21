﻿namespace ABulkCopy.Common.Types.Index;

public class IndexColumn : OrderColumn
{
    public bool IsIncluded { get; set; }

    public IndexColumn Clone()
    {
        return new IndexColumn
        {
            Name = Name,
            Direction = Direction,
            IsIncluded = IsIncluded,
        };
    }
}