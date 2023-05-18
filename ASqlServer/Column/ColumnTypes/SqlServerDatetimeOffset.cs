﻿namespace ASqlServer.Column.ColumnTypes;

public class SqlServerDatetimeOffset : DefaultColumn
{
    public SqlServerDatetimeOffset(int id, string name, bool isNullable, int scale = 7)
        : base(id, name, isNullable)
    {
        Type = ColumnType.DateTimeOffset;
        Scale = scale;
        SetPrecisionAndLength(scale);
    }

    public override string InternalTypeName()
    {
        return "datetime2";
    }

    public override string ToString(object value)
    {
        return Convert.ToDateTime(value).ToString("yyyyMMdd HH:mm:ss.0000000", CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        return DateTime.ParseExact(value, "yyyyMMdd HH:mm:ss.0000000", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }

    private void SetPrecisionAndLength(int scale)
    {
        Precision = 27 + scale;
        Length = Precision switch
        {
            < 30 => 8,
            < 32 => 9,
            _ => 10
        };
    }
}