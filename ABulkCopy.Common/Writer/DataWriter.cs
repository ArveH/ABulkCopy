namespace ABulkCopy.Common.Writer;

public class DataWriter : IDataWriter
{
    private readonly ITableReader _tableReader;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public DataWriter(
        IDbContext dbContext,
        ITableReaderFactory tableReaderFactory,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _tableReader = tableReaderFactory.GetTableReader(dbContext);
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public async Task<long> Write(
        TableDefinition tableDefinition,
        string path)
    {
        var fileFullPath = Path.Combine(
            path, tableDefinition.Header.Name + CommonConstants.DataSuffix);
        await using var writeStream = _fileSystem.File.CreateText(fileFullPath);
        await _tableReader.PrepareReader(tableDefinition);
        var rowCounter = 0;
        while (await _tableReader.Read())
        {
            WriteRow(_tableReader, tableDefinition, writeStream);
            writeStream.WriteLine();
            rowCounter++;
        }

        return rowCounter;
    }

    private void WriteRow(
        ITableReader tableReader, 
        TableDefinition tableDefinition, 
        TextWriter dataWriter)
    {
        for (var i = 0; i < tableDefinition.Columns.Count; i++)
        {
            if (tableReader.IsNull(i))
            {
                dataWriter.Write("NULL,");
                continue;
            }

            if (tableDefinition.Columns[i].Type == ColumnType.Raw)
            {
                WriteBlobColumn(
                    tableDefinition.Header.Name,
                    tableDefinition.Columns[i].Name);
            }
            else
            {
                dataWriter.Write(tableDefinition.Columns[i].ToString(tableReader.GetValue(i)!));
            }
            dataWriter.Write(",");
        }
    }

    private void WriteBlobColumn(string tabName, string colName)
    {
        _logger.Error("Can't write blob column '{ColumnName}' for table '{TableName}'",
            colName, tabName);
        throw new NotImplementedException($"Can't write blob column '{colName}' for table '{tabName}'");
    }
}