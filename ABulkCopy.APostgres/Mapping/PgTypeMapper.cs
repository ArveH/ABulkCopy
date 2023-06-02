namespace ABulkCopy.APostgres.Mapping;

public class PgTypeMapper : ITypeConverter
{
    private readonly IMappingFactory _mappingFactory;

    public PgTypeMapper(IMappingFactory mappingFactory)
    {
        _mappingFactory = mappingFactory;
    }

    public TableDefinition Convert(TableDefinition sourceDefinition)
    {
        if (sourceDefinition.DbServer != DbServer.SqlServer)
        {
            throw new ArgumentException("Only SQL Server schema files can be converted to Postgres");
        }

        var mappings = _mappingFactory.GetDefaultMssToPgMappings();

        var tableDefinition = new TableDefinition(DbServer.Postgres)
        {
            Header = new TableHeader
            {
                Name = sourceDefinition.Header.Name,
                Location = mappings.Locations.SafeGet(sourceDefinition.Header.Location),
                Schema = mappings.Schemas[sourceDefinition.Header.Schema],
            },
            Columns = new List<IColumn>()
        };

        return tableDefinition;
    }
}