namespace ABulkCopy.Common.Reader;

public class ADataReader : IADataReader
{
    public Task<long> Read(TableDefinition tableDefinition, string path)
    {
        throw new NotImplementedException();
    }
}