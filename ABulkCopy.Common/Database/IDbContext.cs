namespace ABulkCopy.Common.Database;

public interface IDbContext
{
    string ConnectionString { get; }
    DbServer DbServer{ get; }
}