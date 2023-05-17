namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerNCharColumn: TemplateSqlServerVarcharColumn
{
    public TemplateSqlServerNCharColumn(string name, int length, bool isNullable, string def, string collation): 
        base(name, length, isNullable, def, collation)
    {
        Type = ColumnType.NChar;
    }

    public override string InternalTypeName()
    {
        return $"nchar({(int) Details["Length"]})";
    }
}