namespace ABulkCopy.Common.Mapping;

public class Mapping : IMapping
{
    public Mapping(
        string name, 
        DbServer sourceDbServer, 
        DbServer targetDbServer)
    {
        Name = name;
        SourceDbServer = sourceDbServer;
        TargetDbServer = targetDbServer;
    }

    public string Name { get; set; }
    public DbServer SourceDbServer { get; set; }
    public DbServer TargetDbServer { get; set; }

    public List<ColumnMap> Columns { get; set; } = new();
    public Dictionary<string, string> Schemas { get; set; } = new();
    public Dictionary<string, string?> Locations { get; set; } = new();
    public Dictionary<string, string?> Collations { get; set; } = new();
}