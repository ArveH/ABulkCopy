﻿namespace ABulkCopy.Common;

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
        public const string ConnectionString = "AppSettings:ConnectionString";
        public const string Folder = "AppSettings:Folder";
        public const string LogFile = "AppSettings:LogFile";
        public const string AddQuotes = "AppSettings:AddQuotes";
        public const string SearchFilter = "AppSettings:SearchFilter";
        public const string EmptyString = "AppSettings:EmptyString";

        public const string PgKeywords = "PgKeywords";
    }
}