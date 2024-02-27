namespace ABulkCopy.Common.Reader;

public interface ISchemaReaderFactory
{
    ISchemaReader Get(Rdbms rdbms);
}