namespace ABulkCopy.Common.Reader;

public interface IDataFileReaderFactory
{
    public IDataFileReader Create(string path, IReadOnlyList<IColumn> columns);
}