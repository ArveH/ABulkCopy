namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerNVarcharColumn: TemplateSqlServerVarcharColumn
{
    public TemplateSqlServerNVarcharColumn(string name, int length, bool isNullable, string def, string collation)
        : base(name, length, isNullable, def, collation)
    {
        if (length == -1)
        {
            Type = ColumnType.NLongText;
            TypeString = "nvarchar(max)";
            Details.Remove("Length");
        }
        else
        {
            Type = ColumnType.NVarchar;
            TypeString = $"nvarchar({length})";
        }
    }
}