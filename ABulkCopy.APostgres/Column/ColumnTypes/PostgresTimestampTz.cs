﻿namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresTimestampTz : DefaultColumn
{
    public PostgresTimestampTz(int id, string name, bool isNullable)
        : base(id, PgTypes.TimestampTz, name, isNullable)
    {
    }

    public override string ToString(object value)
    {
        return ((DateTimeOffset)value).ToString("O");
    }

    public override object ToInternalType(string value)
    {
        return DateTimeOffset.ParseExact(value, "O", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTimeOffset);
    }
}