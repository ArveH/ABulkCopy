﻿namespace ABulkCopy.Common.Mapping;

public interface IMappingFactory
{
    IMapping GetMappings();
    bool ConvertBitToBool { get; }
}