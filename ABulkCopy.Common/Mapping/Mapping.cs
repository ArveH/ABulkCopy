namespace ABulkCopy.Common.Mapping;

public class Mapping : IMapping
{
    public Mapping(
        Dictionary<string, string>? columns = null,
        Dictionary<string, string>? schemas = null,
        Dictionary<string, string?>? locations = null,
        Dictionary<string, string?>? collations = null)
    {
        if (columns != null) Columns = columns;
        if (schemas != null) Schemas = schemas;
        if (locations != null) Locations = locations;
        if (collations != null) Collations = collations;
    }

    public Dictionary<string, string> Columns { get; } = new();
    public Dictionary<string, string> Schemas { get; } = new();
    public Dictionary<string, string?> Locations { get; } = new();
    public Dictionary<string, string?> Collations { get; } = new();
}