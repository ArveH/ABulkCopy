﻿namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresReal : PgDefaultColumn
{
    public PostgresReal(int id, string name, bool isNullable)
        : base(id, PgTypes.Real, name, isNullable)
    {
    }

    public override string ToString(object value)
    {
        return Convert.ToDecimal(value).ToString(CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        return float.Parse(value, CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(float);
    }
}