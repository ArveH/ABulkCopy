using ASqlServer.Column.ColumnTypes;

namespace ASqlServer.Column;

public class MssColumnFactory : IMssColumnFactory
{
    public IColumn Create(
        int id,
        string name,
        string nativeType,
        int? length = null,
        int? precision = null,
        int? scale = null,
        bool isNullable = false,
        string? collation = null,
        DefaultDefinition? defaultDefinition = null,
        Identity? identity = null)
    {
        return new SqlServerBigInt(id, name, isNullable);
    }
}