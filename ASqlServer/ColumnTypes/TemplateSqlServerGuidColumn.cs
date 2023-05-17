namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerGuidColumn : TemplateSqlServerColumn
{
    public TemplateSqlServerGuidColumn(string name, bool isNullable, string def)
        : base(name, ColumnType.Guid, isNullable, false, AdjustDefaultValue(def))
    {
    }

    public override string InternalTypeName()
    {
        return "uniqueidentifier";
    }

    protected string ParseDefaultValue(string def)
    {
        switch (def)
        {
            case "GUID":
                return "newid()";
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
            base.Default = AdjustDefaultValue(value);
        }
    }

    private static string AdjustDefaultValue(string def)
    {
        if (def.IndexOf("newid", StringComparison.Ordinal) >= 0)
        {
            return "GUID";
        }

        return def;
    }

    public override string ToString(object value)
    {
        return ((Guid)value).ToString();
    }

    public override object ToInternalType(string value)
    {
        if (value == null)
        {
            return null;
        }
        return new Guid(value);
    }

    public override Type GetDotNetType()
    {
        return typeof(Guid);
    }
}