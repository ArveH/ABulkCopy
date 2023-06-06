namespace ABulkCopy.APostgres.Reader;

public class PgDataReader : IADataReader
{
    private readonly IPgContext _context;
    private readonly ILogger _logger;

    public PgDataReader(
        IPgContext context,
        ILogger logger)
    {
        _context = context;
        _logger = logger.ForContext<PgDataReader>();
    }

    public async Task<long> Read(TableDefinition tableDefinition, string path)
    {
        _logger.Information("Reading data for table '{TableName}' from '{Path}'",
            tableDefinition.Header.Name, path);
        await using var conn = await _context.DataSource.OpenConnectionAsync().ConfigureAwait(false);

        await using var writer = await conn.BeginBinaryImportAsync(
            CreateCopyStmt(tableDefinition)).ConfigureAwait(false);

        var counter = 0L;
        await ReadRow(writer, tableDefinition.Columns).ConfigureAwait(false);
        counter++;

        await writer.CompleteAsync().ConfigureAwait(false);

        _logger.Information("Read {RowCount} rows for table '{TableName}' from '{Path}'",
            counter, tableDefinition.Header.Name, path);
        return counter;
    }

    private async Task ReadRow(NpgsqlBinaryImporter writer, List<IColumn> columns)
    {
        await writer.StartRowAsync().ConfigureAwait(false);

        foreach (var col in columns)
        {
            await writer.WriteAsync(123, col.Type.GetNativeType()).ConfigureAwait(false);
        }
    }

    private static string CreateCopyStmt(TableDefinition tableDefinition)
    {
        var sb = new StringBuilder();
        sb.Append("COPY ");
        sb.Append($"\"{tableDefinition.Header.Name}\" (");
        var first = true;
        foreach (var column in tableDefinition.Columns)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                sb.Append(",");
            }

            sb.Append($"\"{column.Name}\"");
        }

        sb.Append(") FROM STDIN (FORMAT BINARY)");
        return sb.ToString();
    }
}