using System.Data;

namespace ABulkCopy.Common.Database;

public interface IDbRawReader : IDisposable, IAsyncDisposable
{
    Task<bool> ReadAsync(CancellationToken cancellationToken);
    string GetString(int ordinal);
    int GetInt32(int ordinal);
    decimal GetDecimal(int ordinal);
    bool IsDBNull(int ordinal);
}