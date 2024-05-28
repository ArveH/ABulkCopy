namespace ABulkCopy.APostgres.Reader;

public class PgDataReader : IADataReader, IDisposable
{
    private readonly IPgContext _context;
    private readonly IQueryBuilderFactory _queryBuilderFactory;
    private readonly IDataFileReader _fileReader;
    private readonly ILogger _logger;

    public PgDataReader(
        IPgContext context,
        IQueryBuilderFactory queryBuilderFactory,
        IDataFileReader dataFileReader,
        ILogger logger)
    {
        _context = context;
        _queryBuilderFactory = queryBuilderFactory;
        _fileReader = dataFileReader;
        _logger = logger.ForContext<PgDataReader>();
    }

    public async Task<long> ReadAsync(string folder,
        TableDefinition tableDefinition,
        CancellationToken ct,
        EmptyStringFlag emptyStringFlag = EmptyStringFlag.Leave)
    {
        _logger.Information("Reading data for table '{TableName}' from '{Path}'",
            tableDefinition.GetFullName(), folder);
        await using var conn = await _context.DataSource.OpenConnectionAsync(ct).ConfigureAwait(false);

        var copyStmt = CreateCopyStmt(tableDefinition);
        await using var writer = await conn.BeginBinaryImportAsync(
            copyStmt, ct).ConfigureAwait(false);

        var path = Path.Combine(
            folder, tableDefinition.Data.FileName);
        _fileReader.Open(path);
        var counter = 0L;
        // TODO: Currently, it will not continue on error.
        var errors = 0L;
        while (true)
        {
            if (_fileReader.IsEndOfFile) break;
            try
            {
                await ReadRowAsync(
                    _fileReader, 
                    writer, 
                    folder,
                    tableDefinition,
                    emptyStringFlag,
                    ct).ConfigureAwait(false);
                counter++;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Reading data file for table '{TableName}' failed for row {RowCounter}: {ErrorMessage}",
                    tableDefinition.GetFullName(), counter, ex.FlattenMessages());
                errors++;
                _fileReader.SkipToNextLine();
                break;
            }
        }

        if (errors > 0)
        {
            _logger.Error($"{{ErrorCount}} {"row".Plural(errors)} failed when reading {{RowCount}} {"row".Plural(counter)} for table '{{TableName}}' from '{{Path}}'",
                errors, counter, tableDefinition.GetFullName(), folder);
        }
        else
        {
            await writer.CompleteAsync(ct).ConfigureAwait(false);
            _logger.Information($"Read {{RowCount}} {"row".Plural(counter)} for table '{{TableName}}' from '{{Path}}'",
                counter, tableDefinition.GetFullName(), folder);
        }
        return counter;
    }

    private async Task ReadRowAsync(IDataFileReader dataFileReader,
        NpgsqlBinaryImporter writer,
        string folder,
        TableDefinition tableDefinition,
        EmptyStringFlag emptyStringFlag, 
        CancellationToken ct)
    {
        await writer.StartRowAsync(ct).ConfigureAwait(false);

        foreach (var col in tableDefinition.Columns)
        {
            var colValue = dataFileReader.ReadColumn(col.Name, emptyStringFlag);
            if (colValue == null)
            {
                await writer.WriteNullAsync(ct).ConfigureAwait(false);
                continue;
            }

            if (col.Type.IsRaw())
            {
                var path = Path.Combine(
                    folder,
                    tableDefinition.Data.FileName[..^5], // Remove .data
                    col.Name,
                    colValue);
                await writer.WriteAsync(
                    dataFileReader.ReadAllBytes(path),
                    col.Type.GetNativeType(), 
                    ct).ConfigureAwait(false);
            }
            else
            {
                await writer.WriteAsync(
                    col.ToInternalType(colValue),
                    col.Type.GetNativeType(), 
                    ct).ConfigureAwait(false);
            }
        }

        dataFileReader.ReadNewLine();
    }

    private string CreateCopyStmt(TableDefinition tableDefinition)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        qb.Append("COPY ");
        qb.AppendIdentifier(tableDefinition.Header.Schema);
        qb.Append(".");
        qb.AppendIdentifier(tableDefinition.Header.Name);
        qb.Append(" (");
        var first = true;
        foreach (var column in tableDefinition.Columns)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                qb.Append(",");
            }
            qb.AppendIdentifier(column.Name);
        }

        qb.Append(") FROM STDIN (FORMAT BINARY)");
        return qb.ToString();
    }

    public void Dispose()
    {
        _fileReader.Dispose();
    }
}