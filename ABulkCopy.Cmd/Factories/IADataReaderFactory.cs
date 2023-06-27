namespace ABulkCopy.Cmd.Factories;

public interface IADataReaderFactory
{
    IADataReader Get(Rdbms rdbms);
}