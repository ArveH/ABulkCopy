namespace ABulkCopy.APostgres.Column.ColumnTypes;

public abstract class PgTemplateStrColumn : PgDefaultColumn
{
    protected PgTemplateStrColumn(int id, string type, string name, bool isNullable, int length, string? collation = null)
        : base(id, type, name, isNullable)
    {
        Length = length;
        Collation = collation;
    }

    public override string ToString(object value)
    {
        return InternalToString(value);
    }

    public override object ToInternalType(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? " " : value;
    }

    public override Type GetDotNetType()
    {
        return typeof(string);
    }

    protected string InternalToString(object value, bool shouldTrim = true)
    {
        var cleanValue = Convert.ToString(value)?.Replace(Constants.Quote, $"{Constants.Quote}{Constants.Quote}");
        if (shouldTrim)
        {
            cleanValue = cleanValue?.TrimEnd(' ');
        }
        if (cleanValue == null)
        {
            return "";
        }

        return Constants.Quote + cleanValue + Constants.Quote;
    }
}