namespace ABulkCopy.APostgres.Column;

public interface IPgColumnFactory
{
    IColumn Create(
        int id,
        string name,
        string nativeType,
        int length,
        int? precision = null,
        int? scale = null,
        bool isNullable = false,
        string? collation = null);
}