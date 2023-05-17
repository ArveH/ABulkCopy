using System.Text.RegularExpressions;

namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerVarcharColumn: TemplateSqlServerColumn
{
    protected string TypeString;

    public TemplateSqlServerVarcharColumn(string name, int length, bool isNullable, string def, string collation)
        : base(name, ColumnType.Varchar, isNullable, false, AdjustDefaultValue(def))
    {
        if (length <= 0)
        {
            TypeString = "varchar(max)";
            Type = ColumnType.LongText;
        }
        else
        {
            Details["Length"] = length;
            TypeString = $"varchar({length})";
        }
        Details["Collation"] = collation;
    }

    public override string InternalTypeName()
    {
        return TypeString;
    }

    public override string GetColumnDefinition()
    {
        string collate = "";
        if (!string.IsNullOrWhiteSpace((string)Details["Collation"]))
        {
            collate = string.Format("collate {0} ", (string)Details["Collation"]);
        }
        string defaultValue = "";
        if (!string.IsNullOrEmpty(Default))
        {
            defaultValue = string.Format("default {0} ", Default);
        }
        string notNullConstraint = IsNullable ? "null " : "not null ";

        return string.Format("{0} {1}{2}{3}", InternalTypeName(), collate, defaultValue, notNullConstraint);
    }

    public override string Default
    {
        get { return base.Default; }
        set
        { 
            base.Default = AdjustDefaultValue(value);
        }
    }

    protected static string AdjustDefaultValue(string def)
    {
        if (def.Length >= 4)
        {
            return Regex.Replace(def, @"^\(N?(?<def>.*)\)$", m => m.Groups["def"].Value);
        }

        return def;
    }

    public override string ToString(object value)
    {
        return "'" + Convert.ToString(value).Replace("'", "''").TrimEnd(' ') + "'";
    }

    public override object ToInternalType(string value)
    {
        if (value == null)
        {
            return null;
        }
        return string.IsNullOrEmpty(value) ? " " : value;
    }

    public override Type GetDotNetType()
    {
        return typeof(string);
    }
}