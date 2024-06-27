using INode = AParser.Tree.INode;

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
        pgParser.Modifiers.ConvertBitToBoolean = _mappingFactory.ConvertBitToBool;
    }

    public TableDefinition Convert(TableDefinition sourceDefinition)
    {
        if (sourceDefinition.Rdbms != Rdbms.Mss)
        {
            throw new ArgumentException("Only SQL Server schema files can be converted to Postgres");
        }

        var mappings = _mappingFactory.GetMappings();

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
        tableDefinition.ForeignKeys.ForEach(k => k.SchemaReference = mappings.Schemas[k.SchemaReference]);
        tableDefinition.Indexes.AddRange(sourceDefinition.Indexes.Select(i => i.Clone()));
        tableDefinition.PrimaryKey = sourceDefinition.PrimaryKey?.Clone();

        return tableDefinition;
    }

    private IEnumerable<IColumn> ConvertColumns(TableDefinition sourceDefinition, IMapping mappings)
    {
        var newColumns = new List<IColumn>();
        foreach (var sourceCol in sourceDefinition.Columns)
        {
            var (newPrecision, newScale) = UpdatePrecisionAndScaleForDateAndTimeTypes(sourceCol);

            var newColumn = _columnFactory.Create(
                sourceCol.Id,
                sourceCol.Name,
                mappings.ColumnTypes.GetKeyIfValueNotExist(sourceCol.Type),
                sourceCol.Length,
                newPrecision,
                newScale,
                sourceCol.IsNullable,
                mappings.Collations.NullableGetKeyIfValueNotExist(sourceCol.Collation));
            newColumn.Identity = sourceCol.Identity?.Clone();

            if (sourceCol.DefaultConstraint != null)
            {
                newColumn.DefaultConstraint = new()
                {
                    Name = sourceCol.DefaultConstraint.Name,
                    Definition = SetDefaultConstraint(sourceCol),
                    IsSystemNamed = sourceCol.DefaultConstraint.IsSystemNamed
                };
            }

            newColumn.ComputedDefinition = sourceCol.ComputedDefinition;
            newColumns.Add(newColumn);
        }
        return newColumns;
    }

    private string SetDefaultConstraint(IColumn col)
    {
        var tokenizer = _tokenizerFactory.GetTokenizer();
        tokenizer.Initialize(col.DefaultConstraint!.Definition);
        tokenizer.GetNext();
        var root = _parseTree.CreateExpression(tokenizer);

        if (col.Type != MssTypes.Bit)
        {
            return _pgParser.Parse(tokenizer, root);
        }

        return ChangeNumberOnlyToBoolean(root, tokenizer);
    }

    private string ChangeNumberOnlyToBoolean(INode root, ITokenizer tokenizer)
    {
        var constNode = root.StripParentheses();
        if (_pgParser.Modifiers.ConvertBitToBoolean && constNode != null && constNode.IsSimpleValue())
        {
            return _pgParser.ParseLeafNode(tokenizer, constNode) == "1" ? "true" : "false";
        }

        return _pgParser.Parse(tokenizer, root);
    }

    private static (int? precision, int?scale) UpdatePrecisionAndScaleForDateAndTimeTypes(
        IColumn sourceCol)
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

        return (newPrecision, newScale);
    }
}