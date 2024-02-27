namespace ABulkCopy.Common.Mapping;

public interface IMapping
{
    Dictionary<string, string> Columns { get; }
    Dictionary<string, string> Schemas { get; }
    Dictionary<string, string?> Locations { get; }
    Dictionary<string, string?> Collations { get; }
}