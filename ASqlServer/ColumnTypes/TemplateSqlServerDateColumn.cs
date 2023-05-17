using System.Globalization;

namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerDateColumn: TemplateSqlServerDatetimeColumn
{
    public TemplateSqlServerDateColumn(string name, bool isNullable, string def)
        : base(name, isNullable, ConvertNativeDateToKeyword(def))
    {
        Type = ColumnType.Date;
    }

    public override string InternalTypeName()
    {
        return "date";
    }

    protected override string ParseDefaultValue(string def)
    {
        switch (def)
        {
            case "MIN_DATE":
                return "convert(date,'19000101',9)";
            case "MAX_DATE":
                return "convert(date,'20991231',9)";
            case "TS2DAY(MAX_DATE)":
                return "convert(date,'20991231',9)";
            case "TODAY":
                return "CAST( FLOOR( CAST( getdate() AS FLOAT ) )AS DATE)";
        }

        return def;
    }

    public override string ToString(object value)
    {
        return Convert.ToDateTime(value).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        if (value == null)
        {
            return null;
        }
        return DateTime.ParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture);
    }
}