﻿namespace ABulkCopy.Common.Mapping;

public interface IMappingFactory
{
    IMapping GetDefaultMssToPgMappings();
}