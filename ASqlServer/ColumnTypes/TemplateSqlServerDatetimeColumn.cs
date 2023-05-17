using System.Globalization;

namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerDatetimeColumn : TemplateSqlServerColumn
{
    public TemplateSqlServerDatetimeColumn(string name, bool isNullable, string def)
        : base(name, ColumnType.DateTime, isNullable, false, ConvertNativeDateToKeyword(def))
    {
    }

    public override string InternalTypeName()
    {
        return "datetime";
    }

    protected virtual string ParseDefaultValue(string def)
    {
        switch (def)
        {
            case "MIN_DATE":
                return "convert(datetime,'19000101 00:00:00:000',9)";
            case "MAX_DATE":
                return "convert(datetime,'20991231 23:59:59:998',9)";
            case "TS2DAY(MAX_DATE)":
                return "convert(datetime,'20991231 00:00:00:000',9)";
            case "TODAY":
                return "CAST( FLOOR( CAST( getdate() AS FLOAT ) )AS DATETIME)";
            case "NOW":
                return "getdate()";
        }

        return def;
    }

    public override string GetColumnDefinition()
    {
        string defaultValue = "";
        if (!string.IsNullOrEmpty(Default))
        {
            defaultValue = string.Format("default {0} ", ParseDefaultValue(Default));
        }
        string notNullConstraint = IsNullable ? "null " : "not null ";

        return string.Format("{0} {1}{2}", InternalTypeName(), defaultValue, notNullConstraint);
    }

    public override string Default
    {
        set
        {
            base.Default = ConvertNativeDateToKeyword(value);
        }
    }

    protected static string ConvertNativeDateToKeyword(string date)
    {
        if ((date.IndexOf("19000101", StringComparison.Ordinal) > 0 || date.IndexOf("Jan 1 1900", StringComparison.Ordinal) > 0))
        {
            return "MIN_DATE";
        }
        else if (date.IndexOf("2099", StringComparison.Ordinal) > 0 && ((date.IndexOf("substring", StringComparison.Ordinal) > 0 && date.IndexOf("(1),(8))", StringComparison.Ordinal) > 0 && date.IndexOf("00:00", StringComparison.Ordinal) > 0) || (date.IndexOf("00:00", StringComparison.Ordinal) > 0)))
        {
            return "TS2DAY(MAX_DATE)";
        }
        else if (date.IndexOf("2099", StringComparison.Ordinal) > 0)
        {
            return "MAX_DATE";
        }
        else if (date.IndexOf("floor", StringComparison.Ordinal) > 0 && date.IndexOf("getdate", StringComparison.Ordinal) > 0)
        {
            return "TODAY";
        }
        else if (date.IndexOf("getdate", StringComparison.Ordinal) > 0)
        {
            return "NOW";
        }

        return date;
    }

    public override string ToString(object value)
    {
        return Convert.ToDateTime(value).ToString("yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        if (value == null)
        {
            return null;
        }
        return DateTime.ParseExact(value, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }
}