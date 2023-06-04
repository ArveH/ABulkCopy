namespace ABulkCopy.APostgres.Mapping;

public class PgTypeMapper : ITypeConverter
{
    private readonly IPgColumnFactory _columnFactory;
    private readonly IMappingFactory _mappingFactory;

    public PgTypeMapper(
        IPgColumnFactory columnFactory,
        IMappingFactory mappingFactory)
    {
        _columnFactory = columnFactory;
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
                Location = mappings.Locations.NullGet(sourceDefinition.Header.Location),
                Schema = mappings.Schemas[sourceDefinition.Header.Schema],
            },
            Columns = ConvertColumns(sourceDefinition, mappings).ToList()
        };

        return tableDefinition;
    }

    private IEnumerable<IColumn> ConvertColumns(TableDefinition sourceDefinition, IMapping mappings)
    {
        return sourceDefinition.Columns
            .Select(column => _columnFactory.Create(
                column.Id, 
                column.Name, 
                mappings.Columns.ReplaceGet(column.Type), 
                column.Length, 
                column.Precision, 
                column.Scale, 
                column.IsNullable, 
                column.Collation));
    }
}