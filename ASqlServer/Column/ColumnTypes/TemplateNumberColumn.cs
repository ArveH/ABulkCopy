namespace ASqlServer.Column.ColumnTypes;

public abstract class TemplateNumberColumn : TemplateSqlServerColumn
{
    protected TemplateNumberColumn(
        int id, string name, bool isNullable, int length, int precision = 0, int scale = 0)
        : base(id, name, isNullable)
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

    public override Type GetDotNetType()
    {
        return typeof(int);
    }
}