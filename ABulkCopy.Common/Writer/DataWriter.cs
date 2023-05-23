namespace ABulkCopy.Common.Writer;

public class DataWriter : IDataWriter
{
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public DataWriter(

        IFileSystem fileSystem,
        ILogger logger)
    {
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public async Task WriteTable(
        ITableReader tableReader,
        TableDefinition tableDefinition,
        string path)
    {
        var fileFullPath = Path.Combine(
            path, tableDefinition.Header.Name + CommonConstants.DataSuffix);
        await using var writeStream = _fileSystem.File.CreateText(fileFullPath);
        await tableReader.PrepareReader(tableDefinition);
        while (await tableReader.Read())
        {
            WriteRow(tableReader, tableDefinition, writeStream);
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