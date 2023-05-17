namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerNTextColumn : TemplateSqlServerVarcharColumn
{
    public TemplateSqlServerNTextColumn(string name, bool isNullable, string def, string collation)
        : base(name, -1, isNullable, AdjustDefaultValue(def), collation)
    {
        Type = ColumnType.NOldText;
    }

    public override string InternalTypeName()
    {
        return "ntext";
    }
}