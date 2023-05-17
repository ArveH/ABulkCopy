namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerBinaryColumn : TemplateSqlServerColumn
{
    private readonly string _typeString;
    public TemplateSqlServerBinaryColumn(string name, int length, bool isNullable, string def)
        : base(name, ColumnType.Raw, isNullable, false, def)
    {
        Details["Length"] = length;
        _typeString =  $"binary({length})";
    }

    public override string InternalTypeName()
    {
        return _typeString;
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
        return Convert.ToBase64String((byte[])value);
    }
        
    public override object ToInternalType(string value)
    {
        if (value == null)
        {
            return DBNull.Value;
        }
        return Convert.FromBase64String(value);
    }

    public override Type GetDotNetType()
    {
        return typeof(byte[]);
    }
}