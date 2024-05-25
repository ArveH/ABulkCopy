namespace ABulkCopy.APostgres.Mapping;

public class PgTypeMapper : ITypeConverter
{
    private readonly IPgParser _pgParser;
    private readonly IParseTree _parseTree;
    private readonly ITokenizerFactory _tokenizerFactory;
    private readonly IPgColumnFactory _columnFactory;
    private readonly IMappingFactory _mappingFactory;

    public PgTypeMapper(
        IPgParser pgParser,
        IParseTree parseTree,
        ITokenizerFactory tokenizerFactory,
        IPgColumnFactory columnFactory,
        IMappingFactory mappingFactory)
    {
        _pgParser = pgParser;
        _parseTree = parseTree;
        _tokenizerFactory = tokenizerFactory;
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
                Location = mappings.Locations.NullableGet(sourceDefinition.Header.Location),
                Schema = mappings.Schemas[sourceDefinition.Header.Schema],
            },
            Data = new TableData { FileName = sourceDefinition.Data.FileName },
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
                mappings.Columns.GetKeyIfValueNotExist(sourceCol.Type),
                sourceCol.Length,
                newPrecision,
                newScale,
                sourceCol.IsNullable,
                mappings.Collations.NullableGetKeyIfValueNotExist(sourceCol.Collation));
            newColumn.Identity = sourceCol.Identity?.Clone();

            if (sourceCol.DefaultConstraint != null)
            {
                var tokenizer = _tokenizerFactory.GetTokenizer();
                tokenizer.Initialize(sourceCol.DefaultConstraint!.Definition);
                tokenizer.GetNext();
                var root = _parseTree.CreateExpression(tokenizer);
                newColumn.DefaultConstraint = new()
                {
                    Name = sourceCol.DefaultConstraint.Name,
                    Definition = _pgParser.Parse(tokenizer, root),
                    IsSystemNamed = sourceCol.DefaultConstraint.IsSystemNamed
                };
            }

            newColumn.ComputedDefinition = sourceCol.ComputedDefinition;
            newColumns.Add(newColumn);
        }
        return newColumns;
    }
}