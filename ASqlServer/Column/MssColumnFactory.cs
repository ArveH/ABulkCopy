using ASqlServer.Column.ColumnTypes;

namespace ASqlServer.Column;

public class MssColumnFactory : IMssColumnFactory
{
    private readonly ILogger _logger;

    public MssColumnFactory(ILogger logger)
    {
        _logger = logger;
    }

    public IColumn Create(
        int id,
        string name,
        string nativeType,
        int length,
        int? precision = null,
        int? scale = null,
        bool isNullable = false,
        string? collation = null)
    {
        if (nativeType == "bigint")
        {
            return new SqlServerBigInt(id, name, isNullable);
        }
        if (nativeType == "binary")
        {
            return new SqlServerBinary(id, name, isNullable, length);
        }
        if (nativeType == "bit")
        {
            return new SqlServerBit(id, name, isNullable);
        }

        if (nativeType == "char")
        {
            return new SqlServerChar(id, name, isNullable, length, collation);
        }

        if (nativeType == "date")
        {
            return new SqlServerDate(id, name, isNullable);
        }

        if (nativeType == "datetime")
        {
            return new SqlServerDatetime(id, name, isNullable);
        }

        if (nativeType == "datetime2")
        {
            return new SqlServerDatetime2(id, name, isNullable, scale);
        }

        if (nativeType == "datetimeoffset")
        {
            return new SqlServerDatetimeOffset(id, name, isNullable, scale);
        }

        if (nativeType == "decimal" || nativeType == "numeric")
        {
            return new SqlServerDecimal(id, name, isNullable, 
                precision??throw new ArgumentNullException(nameof(precision), "precision can't be null for Decimal"), 
                scale);
        }

        if (nativeType == "float")
        {
            return new SqlServerFloat(id, name, isNullable);
        }

        if (nativeType == "int")
        {
            return new SqlServerInt(id, name, isNullable);
        }
        if (nativeType == "money")
        {
            return new SqlServerMoney(id, name, isNullable);
        }
        if (nativeType == "nchar")
        {
            return new SqlServerNChar(id, name, isNullable, length, collation);
        }
        if (nativeType == "nvarchar")
        {
            return new SqlServerNVarChar(id, name, isNullable, length, collation);
        }
        if (nativeType == "real")
        {
            return new SqlServerReal(id, name, isNullable);
        }
        if (nativeType == "smalldatetime")
        {
            return new SqlServerSmallDatetime(id, name, isNullable);
        }
        if (nativeType == "smallint")
        {
            return new SqlServerSmallInt(id, name, isNullable);
        }
        if (nativeType == "smallmoney")
        {
            return new SqlServerSmallMoney(id, name, isNullable);
        }
        if (nativeType == "time")
        {
            return new SqlServerTime(id, name, isNullable, scale);
        }

        if (nativeType == "timestamp")
        {
            return new SqlServerTimestamp(id, name, isNullable);
        }

        if (nativeType == "tinyint")
        {
            return new SqlServerTinyInt(id, name, isNullable);
        }

        if (nativeType == "uniqueidentifier")
        {
            return new SqlServerUniqueIdentifier(id, name, isNullable);
        }

        if (nativeType == "varbinary")
        {
            return new SqlServerVarBinary(id, name, isNullable, length);
        }

        if (nativeType == "varchar")
        {
            return new SqlServerVarChar(id, name, isNullable, length, collation);
        }

        if (nativeType == "xml")
        {
            return new SqlServerXml(id, name, isNullable);
        }

        _logger.Error("Unhandled native type '{NativeType}'", nativeType);
        throw new ArgumentOutOfRangeException(nameof(nativeType), $"Unhandled native type '{nativeType}'");
    }
}