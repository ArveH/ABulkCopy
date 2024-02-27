namespace ABulkCopy.Common.Database;

public interface IDbContext
{
    string ConnectionString { get; }
    Rdbms Rdbms{ get; }
    int MaxIdentifierLength { get; }
}