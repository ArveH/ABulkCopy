using System.Globalization;

namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerTimeColumn: TemplateSqlServerColumn
{
    public TemplateSqlServerTimeColumn(string name, bool isNullable, string def)
        : base(name, ColumnType.Time, isNullable, false, def)
    {
        Type = ColumnType.Time;
    }

    public override string InternalTypeName()
    {
        return "time";
    }

    public override string GetColumnDefinition()
    {
        var defaultValue = "";
        if (!string.IsNullOrEmpty(Default))
        {
            defaultValue = $"default {Default} ";
        }
        var notNullConstraint = IsNullable ? "null " : "not null ";

        return $"{InternalTypeName()} {defaultValue}{notNullConstraint}";
    }

    public override string ToString(object value)
    {
        var timeSpan = (TimeSpan)value;
        return timeSpan.ToString("c", CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        if (value == null)
        {
            return null;
        }
        return TimeSpan.ParseExact(value, "c", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(TimeSpan);
    }
}