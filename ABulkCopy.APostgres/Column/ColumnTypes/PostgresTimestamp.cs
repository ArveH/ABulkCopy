namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresTimestamp : PgDefaultColumn
{
    public PostgresTimestamp(int id, string name, bool isNullable, int? precision = null)
        : base(id, PgTypes.Timestamp, name, isNullable)
    {
        Precision = precision is null or > 6 or < 0 ? 6 : precision;
    }

    public override string GetTypeClause()
    {
        return Precision == 6 ? $"{Type}" : $"{Type}({Precision})";
    }

    public override string ToString(object value)
    {
        return ((DateTime)value).ToString("O");
    }

    public override object ToInternalType(string value)
    {
        return DateTime.Parse(value, CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }

    // TODO: Remove
    public string GetDefaultClause()
    {
        if (DefaultConstraint == null) return "";

        var dateStr = DefaultConstraint.Definition.ExtractSingleQuoteString();

        if (DefaultConstraint.Definition.Contains("Convert", StringComparison.InvariantCultureIgnoreCase) &&
            dateStr != null)
        {
            var longDate = dateStr.ExtractLongDateString();
            if (longDate == null || longDate.EndsWith(":000'"))
            {
                // CAST doesn't accept strings with milliseconds
                return $" DEFAULT CAST({dateStr.Replace(":000", "")} AS timestamp)";
            }
            return $" DEFAULT to_timestamp({longDate}, 'YYYYMMDD HH24:MI:SS:FF3')";
        }

        if (DefaultConstraint.Definition.Contains("getdate", StringComparison.InvariantCultureIgnoreCase))
        {
            return " DEFAULT CURRENT_DATE";
        }

        return $" DEFAULT {DefaultConstraint.Definition}";
    }
}