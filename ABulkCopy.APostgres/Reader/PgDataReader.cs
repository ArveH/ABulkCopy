namespace ABulkCopy.APostgres.Reader;

public class PgDataReader : IADataReader
{
    private readonly IPgContext _context;
    private readonly IDataFileReader _fileReader;
    private readonly ILogger _logger;

    public PgDataReader(
        IPgContext context,
        IDataFileReader dataFileReader,
        ILogger logger)
    {
        _context = context;
        _fileReader = dataFileReader;
        _logger = logger.ForContext<PgDataReader>();
    }

    public async Task<long> Read(string folder, TableDefinition tableDefinition)
    {
        _logger.Information("Reading data for table '{TableName}' from '{Path}'",
            tableDefinition.Header.Name, folder);
        await using var conn = await _context.DataSource.OpenConnectionAsync().ConfigureAwait(false);

        await using var writer = await conn.BeginBinaryImportAsync(
            CreateCopyStmt(tableDefinition)).ConfigureAwait(false);

        var path = Path.Combine(
            folder,
            $"{tableDefinition.Header.Name}{tableDefinition.DbServer.DataSuffix()}");
        _fileReader.Open(path);
        var counter = 0L;
        while (true)
        {
            await ReadRow(
                _fileReader, 
                writer, 
                folder,
                tableDefinition).ConfigureAwait(false);
            counter++;
            if (_fileReader.IsEndOfFile) break;
        }

        await writer.CompleteAsync().ConfigureAwait(false);

        _logger.Information($"Read {{RowCount}} {"row".Plural(counter)} for table '{{TableName}}' from '{{Path}}'",
            counter, tableDefinition.Header.Name, folder);
        return counter;
    }

    private async Task ReadRow(
        IDataFileReader dataFileReader,
        NpgsqlBinaryImporter writer,
        string folder,
        TableDefinition tableDefinition)
    {
        await writer.StartRowAsync().ConfigureAwait(false);

        foreach (var col in tableDefinition.Columns)
        {
            var colValue = dataFileReader.ReadColumn(col.Name);
            if (colValue == null)
            {
                await writer.WriteNullAsync().ConfigureAwait(false);
                continue;
            }

            if (col.Type.IsRaw())
            {
                var path = Path.Combine(
                    folder,
                    tableDefinition.Header.Name,
                    col.Name,
                    colValue);
                await writer.WriteAsync(
                    dataFileReader.ReadAllBytes(path),
                    col.Type.GetNativeType()).ConfigureAwait(false);
            }
            else
            {
                await writer.WriteAsync(
                    col.ToInternalType(colValue),
                    col.Type.GetNativeType()).ConfigureAwait(false);
            }
        }

        dataFileReader.ReadNewLine();
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