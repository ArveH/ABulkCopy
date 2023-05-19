namespace ASqlServer.Column.ColumnTypes;

public class SqlServerXml : DefaultColumn
{
    public SqlServerXml(int id, string name, bool isNullable)
        : base(id, name, isNullable)
    {
    }

    public override string GetNativeType()
    {
        return "xml";
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