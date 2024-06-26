﻿namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresMoney : PgDefaultColumn
{
    public PostgresMoney(
        int id, string name, bool isNullable)
        : base(id, PgTypes.Money, name, isNullable)
    {
    }

    public override string ToString(object value)
    {
        return Convert.ToDecimal(value).ToString("0.####", CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        return decimal.Parse(value, CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(decimal);
    }
}