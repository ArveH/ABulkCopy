namespace ABulkCopy.Common.Database;

public interface IDbRawCommand
{
    Task ExecuteNonQueryAsync(
        string sqlString, 
        CancellationToken ct);
    
    Task<object?> ExecuteScalarAsync(
        string sqlString, 
        CancellationToken ct);
    
    Task<TReturn?> ExecuteQueryAsync<TReturn>(
        string sqlString,
        Func<IDbRawReader, Task<TReturn?>> func,
        CancellationToken ct);

    Task<IEnumerable<TReturn>> ExecuteQueryAsync<TReturn>(
        string sqlString,
        Func<IDbRawReader, Task<IEnumerable<TReturn>>> func,
        CancellationToken ct);
    
    Task ExecuteReaderAsync(
        DbCommand command,
        Action<IDbRawReader> readFunc,
        CancellationToken ct);

    Task ExecuteReaderAsync(
        DbCommand command,
        Func<IDbRawReader, Task> readFunc,
        CancellationToken ct);
}