namespace ASqlServer.Column;

public class MssColumnTypeConverter
{
    public ColumnType ToBulkCopyType(
        string type, int? length = null, int? precision = null, int? scale = null)
    {
        return type switch
        {
            "bigint" => ColumnType.BigInt,
            "binary" => ColumnType.SmallRaw,
            "bit" => ColumnType.Bool,
            "char" => ColumnType.Char,
            "date" => ColumnType.Date,
            "datetime" or "datetime2" => ColumnType.DateTime,
            "datetimeoffset" => ColumnType.DateTimeOffset,
            "decimal" => ColumnType.Decimal,
            "float" => ColumnType.Float,
            "int" => ColumnType.Int,
            "money" => ColumnType.Money,
            "nchar" => ColumnType.NChar,
            "numeric" => ColumnType.Decimal,
            "nvarchar" => ColumnType.NVarChar,
            "real" => ColumnType.SmallFloat,
            "smalldatetime" => ColumnType.SmallDateTime,
            "smallint" => ColumnType.SmallInt,
            "smallmoney" => ColumnType.SmallMoney,
            "time" => ColumnType.Time,
            "timestamp" => ColumnType.TimeStamp,
            "tinyint" => ColumnType.TinyInt,
            "uniqueidentifier" => ColumnType.Guid,
            "varbinary" => ColumnType.Raw,
            "varchar" => ColumnType.VarChar,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}