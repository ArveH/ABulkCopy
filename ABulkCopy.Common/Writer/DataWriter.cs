namespace ABulkCopy.Common.Writer;

public class DataWriter : IDataWriter
{
    private readonly ITableReader _tableReader;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public DataWriter(
        ITableReader tableReader,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _tableReader = tableReader;
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public async Task WriteTable(
        TableDefinition tableDefinition,
        string path)
    {
        var fileFullPath = Path.Combine(
            path, tableDefinition.Header.Name + CommonConstants.DataSuffix);
        await using var writeStream = _fileSystem.File.CreateText(fileFullPath);
        await _tableReader.PrepareReader(tableDefinition);
        while (await _tableReader.Read())
        {
            WriteRow(_tableReader, tableDefinition, writeStream);
            writeStream.WriteLine();
        }
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
                WriteBlobColumn();
            }
            else
            {
                dataWriter.Write(tableDefinition.Columns[i].ToString(tableReader.GetValue(i)!));
            }
            dataWriter.Write(",");
        }
    }

    private void WriteBlobColumn()
    {
        throw new NotImplementedException();
    }
}