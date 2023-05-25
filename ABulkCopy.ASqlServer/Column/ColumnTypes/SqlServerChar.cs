namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerChar : TemplateStrColumn
{
    public SqlServerChar(int id, string name, bool isNullable, int length, string? collation = null)
        : base(id, name, isNullable, length, collation)
    {
        Type = ColumnType.Char;
    }

    public override string ToString(object value)
    {
        var cleanValue = Convert.ToString(value)?.Replace("'", "''");
        if (cleanValue == null)
        {
            return "NULL";
        }

        return "'" + cleanValue + "'";
    }

    public override string GetNativeType()
    {
        return $"char({Length})";
    }
}