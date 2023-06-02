namespace ABulkCopy.Common.Mapping;

public interface IMapping
{
    string Name { get; set; }
    DbServer SourceDbServer { get; set; }
    DbServer TargetDbServer { get; set; }
    List<ColumnMap> Columns { get; set; }
    Dictionary<string, string> Schemas { get; set; }
    Dictionary<string, string?> Locations { get; set; }
    Dictionary<string, string?> Collations { get; set; }
}