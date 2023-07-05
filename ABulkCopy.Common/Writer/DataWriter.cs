namespace ABulkCopy.Common.Writer;

public class DataWriter : IDataWriter
{
    private readonly IDbContext _dbContext;
    private readonly ITableReaderFactory _tableReaderFactory;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public DataWriter(
        IDbContext dbContext,
        ITableReaderFactory tableReaderFactory,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _dbContext = dbContext;
        _tableReaderFactory = tableReaderFactory;
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public async Task<long> Write(
    TableDefinition tableDefinition,
        string path)
    {
        var tableReader = _tableReaderFactory.GetTableReader(_dbContext);
        AddDirectoriesForBlobs(tableDefinition, path);
        var fileFullPath = Path.Combine(
            path, tableDefinition.Header.Name + Constants.DataSuffix);
        await using var streamWriter = _fileSystem.File.CreateText(fileFullPath);
        await tableReader.PrepareReader(tableDefinition);
        var rowCounter = 0;
        while (await tableReader.Read())
        {
            WriteRow(tableReader, tableDefinition, streamWriter, path, rowCounter);
            streamWriter.WriteLine();
            rowCounter++;
        }

        return rowCounter;
    }

    private void AddDirectoriesForBlobs(TableDefinition tableDefinition, string path)
    {
        if (!_fileSystem.Directory.Exists(path))
        {
            throw new InvalidOperationException($"Directory '{path}' does not exist");
        }

        foreach (var column in tableDefinition.Columns)
        {
            if (column.Type.IsRaw())
            {
                var blobPath = Path.Combine(path, tableDefinition.Header.Name, column.Name);
                if (!_fileSystem.Directory.Exists(blobPath))
                {
                    _logger.Information("Creating directory '{blobPath}' for blob column '{columnName}'",
                        blobPath, column.Name);
                    _fileSystem.Directory.CreateDirectory(blobPath);
                }
            }
        }
    }

    private void WriteRow(ITableReader tableReader,
        TableDefinition tableDefinition,
        TextWriter textWriter,
        string path,
        int rowCounter)
    {
        for (var i = 0; i < tableDefinition.Columns.Count; i++)
        {
            if (tableReader.IsNull(i))
            {
                textWriter.Write(Constants.ColumnSeparator);
                continue;
            }

            if (tableDefinition.Columns[i].Type.IsRaw())
            {
                var rawFileName = $"i{rowCounter:D15}.raw";
                textWriter.Write(rawFileName);
                WriteBlobColumn(
                    i,
                    Path.Combine(
                        path,
                        tableDefinition.Header.Name,
                        tableDefinition.Columns[i].Name,
                        rawFileName)
                    ,
                    tableReader);
            }
            else
            {
                textWriter.Write(tableDefinition.Columns[i].ToString(tableReader.GetValue(i)!));
            }

            textWriter.Write(Constants.ColumnSeparator);
        }
    }

    private const int RawBufferSize = 4096;

    private void WriteBlobColumn(int colId, string fullPath, ITableReader tableReader)
    {
        var buf = new byte[RawBufferSize];
        using var writer = new BinaryWriter(_fileSystem.FileStream.New(fullPath, FileMode.Create, FileAccess.Write));
        var startIndex = 0L;
        var byteCount = tableReader.GetBytes(colId, startIndex, buf, RawBufferSize);
        while (byteCount == RawBufferSize)
        {
            writer.Write(buf, 0, (int)byteCount);
            writer.Flush();
            startIndex += byteCount;
            byteCount = tableReader.GetBytes(colId, startIndex, buf, RawBufferSize);
        }

        writer.Write(buf, 0, (int)byteCount);
        writer.Flush();
    }
}