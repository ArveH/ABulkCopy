namespace ABulkCopy.Common.Database;

public interface IDbRawReader
{
    Task<bool> ReadAsync(CancellationToken cancellationToken);
    string GetString(int ordinal);
}