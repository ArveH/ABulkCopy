namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerCharColumn: TemplateSqlServerVarcharColumn
{
    public TemplateSqlServerCharColumn(string name, int length, bool isNullable, string def, string collation): base(name, length, isNullable, def, collation)
    {
        Type = ColumnType.Char;
    }

    public override string InternalTypeName()
    {
        return string.Format("char({0})", (int)Details["Length"]);
    }
}