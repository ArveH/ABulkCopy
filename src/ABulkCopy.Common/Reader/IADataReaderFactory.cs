namespace ABulkCopy.Common.Reader;

public interface IADataReaderFactory
{
    IADataReader Get(Rdbms rdbms);
}