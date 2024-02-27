namespace ABulkCopy.Common.Reader;

public interface ITableReaderFactory
{
    ITableReader GetTableReader(IDbContext dbContext);
}