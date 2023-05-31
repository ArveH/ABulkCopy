namespace ABulkCopy.Common.Types.Column;

public abstract class TemplateStrColumn : DefaultColumn
{
    protected TemplateStrColumn(int id, string type, string name, bool isNullable, int length, string? collation = null)
        : base(id, type, name, isNullable)
    {
        Length = length;
        Collation = collation;
    }

    public override string ToString(object value)
    {
        var cleanValue = Convert.ToString(value)?.Replace("'", "''").TrimEnd(' ');
        if (cleanValue == null)
        {
            return "NULL";
        }

        return "'" + cleanValue + "'";
    }

    public override object ToInternalType(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? " " : value;
    }

    public override Type GetDotNetType()
    {
        return typeof(string);
    }
}