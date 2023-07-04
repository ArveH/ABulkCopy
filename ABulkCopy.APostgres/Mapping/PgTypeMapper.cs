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
        if (sourceDefinition.Rdbms != Rdbms.Mss)
        {
            throw new ArgumentException("Only SQL Server schema files can be converted to Postgres");
        }

        var mappings = _mappingFactory.GetDefaultMssToPgMappings();

        var tableDefinition = new TableDefinition(Rdbms.Pg)
        {
            Header = new TableHeader
            {
                Name = sourceDefinition.Header.Name,
                Location = mappings.Locations.NullGet(sourceDefinition.Header.Location),
                Schema = mappings.Schemas[sourceDefinition.Header.Schema],
            },
            Columns = ConvertColumns(sourceDefinition, mappings).ToList()
        };
        tableDefinition.ForeignKeys.AddRange(sourceDefinition.ForeignKeys.Select(k => k.Clone()));
        tableDefinition.Indexes.AddRange(sourceDefinition.Indexes.Select(i => i.Clone()));
        tableDefinition.PrimaryKey = sourceDefinition.PrimaryKey?.Clone();

        return tableDefinition;
    }

    private IEnumerable<IColumn> ConvertColumns(TableDefinition sourceDefinition, IMapping mappings)
    {
        var newColumns = new List<IColumn>();
        foreach (var sourceCol in sourceDefinition.Columns)
        {
            var newScale = sourceCol.Scale;
            var newPrecision = sourceCol.Precision;
            switch (sourceCol.Type)
            {
                case MssTypes.Time:
                case MssTypes.DateTime:
                case MssTypes.DateTime2:
                case MssTypes.DateTimeOffset:
                    newPrecision = sourceCol.Scale;
                    newScale = null;
                    break;
            }

            var newColumn = _columnFactory.Create(
                sourceCol.Id,
                sourceCol.Name,
                mappings.Columns.ReplaceGet(sourceCol.Type),
                sourceCol.Length,
                newPrecision,
                newScale,
                sourceCol.IsNullable,
                mappings.Collations.ReplaceGetNull(sourceCol.Collation));
            newColumn.Identity = sourceCol.Identity?.Clone();
            newColumn.DefaultConstraint = sourceCol.DefaultConstraint?.Clone();
            newColumn.ComputedDefinition = sourceCol.ComputedDefinition;
            newColumns.Add(newColumn);
        }
        return newColumns;
    }
}