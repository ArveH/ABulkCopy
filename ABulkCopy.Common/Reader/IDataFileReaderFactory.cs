namespace ABulkCopy.Common.Reader;

public interface IDataFileReaderFactory
{
    public IDataFileReader Create(string folder, TableDefinition tableDefinition);
}