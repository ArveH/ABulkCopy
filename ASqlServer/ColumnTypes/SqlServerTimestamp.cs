﻿namespace ASqlServer.ColumnTypes;

public class SqlServerTimestamp: TemplateSqlServerColumn
{
    public SqlServerTimestamp(int id, string name, bool isNullable)
        : base(id, name, isNullable)
    {
        Type = ColumnType.TimeStamp;
        Length = 8;
    }

    public override string InternalTypeName()
    {
        return "timestamp";
    }

    public override string ToString(object value)
    {
        return Convert.ToBase64String((byte[])value);
    }

    public override object ToInternalType(string value)
    {
        return Convert.FromBase64String(value);
    }

    public override Type GetDotNetType()
    {
        return typeof(byte[]);
    }
}