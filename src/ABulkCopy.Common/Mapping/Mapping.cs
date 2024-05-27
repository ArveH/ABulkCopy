namespace ABulkCopy.Common.Mapping;

public class Mapping : IMapping
{
    public Mapping(
        Dictionary<string, string>? columns = null,
        Dictionary<string, string>? schemas = null,
        Dictionary<string, string?>? locations = null,
        Dictionary<string, string?>? collations = null)
    {
        Columns = columns ?? new();
        Schemas = schemas ?? new();
        Locations = locations ?? new();
        Collations = collations ?? new();
    }

    public Dictionary<string, string> Columns { get; }
    public Dictionary<string, string> Schemas { get; }
    public Dictionary<string, string?> Locations { get; }
    public Dictionary<string, string?> Collations { get; }
}