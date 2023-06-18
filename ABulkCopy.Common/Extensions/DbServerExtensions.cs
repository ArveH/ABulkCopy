namespace ABulkCopy.Common.Extensions;

public static class DbServerExtensions
{
    public static string SchemaSuffix(this Rdbms rdbms)
    {
        var dbSuffix = rdbms == Rdbms.Mss ? "mss" : "pg";
        return $".{dbSuffix}.schema";
    }

    public static string DataSuffix(this Rdbms rdbms)
    {
        var dbSuffix = rdbms == Rdbms.Mss ? "mss" : "pg";
        return $".{dbSuffix}.data";
    }
}