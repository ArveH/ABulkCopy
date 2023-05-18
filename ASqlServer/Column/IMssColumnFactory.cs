namespace ASqlServer.Column;

public interface IMssColumnFactory
{
    IColumn Create(
        int id,
        string name,
        string nativeType,
        int? length = null,
        int? precision = null,
        int? scale = null,
        bool isNullable = false,
        string? collation = null,
        DefaultDefinition? defaultDefinition = null,
        Identity? identity = null);
}