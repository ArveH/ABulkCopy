namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerSmallMoneyColumn: TemplateSqlServerMoneyColumn
{
    public TemplateSqlServerSmallMoneyColumn(string name, bool isNullable)
        : base(name, isNullable)
    {
        Type = ColumnType.SmallMoney;
    }

    public override string InternalTypeName()
    {
        return "smallmoney";
    }
}