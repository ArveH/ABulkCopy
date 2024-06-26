﻿namespace AParser;

public class SqlTypes : ISqlTypes
{
    private static readonly HashSet<string> TypeNames = new(StringComparer.InvariantCultureIgnoreCase)
    {
        "bit",
        "datetime",
        "smalldatetime"
    };

    public bool Exist(string name)
    {
        return TypeNames.Contains(name);
    }
}