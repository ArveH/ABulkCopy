namespace ABulkCopy.Common.Extensions;

public static class DbServerExtensions
{
    public static string SchemaSuffix(this DbServer dbServer)
    {
        var dbSuffix = dbServer == DbServer.SqlServer ? "mss" : "pg";
        return $".{dbSuffix}.schema";
    }

    public static string DataSuffix(this DbServer dbServer)
    {
        var dbSuffix = dbServer == DbServer.SqlServer ? "mss" : "pg";
        return $".{dbSuffix}.data";
    }
}