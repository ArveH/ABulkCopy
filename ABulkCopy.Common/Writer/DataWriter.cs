namespace ABulkCopy.Common.Writer;

public class DataWriter
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
        StreamWriter writeStream)
    {
        throw new NotImplementedException();
    }
}