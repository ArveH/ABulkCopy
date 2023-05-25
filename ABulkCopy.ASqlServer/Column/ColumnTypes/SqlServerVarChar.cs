namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerVarChar : TemplateStrColumn
{
    public SqlServerVarChar(int id, string name, bool isNullable, int length, string? collation = null)
        : base(id, name, isNullable, length, collation)
    {
        if (length > 4000)
        {
            Type = ColumnType.LongText;
            Length = -1;
        }
        else
        {
            Type = ColumnType.VarChar;
            Length = length;
        }
    }

    public override string GetNativeType()
    {
        return Length == -1 ? "varchar(max)" : $"varchar({Length})";
    }
}