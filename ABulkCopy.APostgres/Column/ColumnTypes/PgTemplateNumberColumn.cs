namespace ABulkCopy.APostgres.Column.ColumnTypes;

public abstract class PgTemplateNumberColumn : PgDefaultColumn
{
    protected PgTemplateNumberColumn(
        int id, string type, string name, bool isNullable, int length, int precision = 0, int scale = 0)
        : base(id, type, name, isNullable)
    {
        Length = length;
        Precision = precision;
        Scale = scale;
    }

    public override string ToString(object value)
    {
        return Convert.ToInt32(value).ToString();
    }

    public override object ToInternalType(string value)
    {
        return Convert.ToInt32(value);
    }
}