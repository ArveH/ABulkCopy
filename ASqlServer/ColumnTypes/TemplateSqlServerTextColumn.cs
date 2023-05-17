namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerTextColumn: TemplateSqlServerVarcharColumn
{
    public TemplateSqlServerTextColumn(string name, bool isNullable, string def, string collation)
        : base(name, -1, isNullable, AdjustDefaultValue(def), collation)
    {
        Type = ColumnType.OldText;
    }

    public override string InternalTypeName()
    {
        return "text";
    }
}