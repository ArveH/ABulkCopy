namespace ASqlServer.ColumnTypes;

public class TemplateNumberColumn : TemplateSqlServerColumn
{
    public TemplateNumberColumn(
        int id, string name, bool isNullable, int length, int precision=0, int scale=0)
        : base(id, name)
    {
        IsNullable = isNullable;
        Length = length;
        Precision = precision;
        Scale = scale;
    }

    public override string InternalTypeName()
    {
        return "int";
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