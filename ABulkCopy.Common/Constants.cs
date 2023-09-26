namespace ABulkCopy.Common;

public static class Constants
{
    public const string SchemaSuffix = ".schema";
    public const string DataSuffix = ".data";

    public const char QuoteChar = '\'';
    public const char ColumnSeparatorChar = ',';
    public const string Quote = "'";
    public const string ColumnSeparator = ",";

    public struct Config
    {
        public const string DbKey = "BulkCopy";
        public const string AddQuotes = "AddQuotes";
        public const string QuoteIdentifiers = "QuoteIdentifiers";
    }
}