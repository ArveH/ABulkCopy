namespace ABulkCopy.ASqlServer.Column;

public class MssColumnFactory : IMssColumnFactory
{
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
        switch (nativeType)
        {
            case MssTypes.BigInt:
                return new SqlServerBigInt(id, name, isNullable);
            case MssTypes.Binary:
                return new SqlServerBinary(id, name, isNullable, length);
            case MssTypes.Bit:
                return new SqlServerBit(id, name, isNullable);
            case MssTypes.Char:
                return new SqlServerChar(id, name, isNullable, length, collation);
            case MssTypes.Date:
                return new SqlServerDate(id, name, isNullable);
            case MssTypes.DateTime:
                return new SqlServerDateTime(id, name, isNullable);
            case MssTypes.DateTime2:
                return new SqlServerDateTime2(id, name, isNullable, scale);
            case MssTypes.DateTimeOffset:
                return new SqlServerDateTimeOffset(id, name, isNullable, scale);
            case MssTypes.Decimal:
            case MssTypes.Numeric:
                return new SqlServerDecimal(id, name, isNullable, 
                    precision??throw new ArgumentNullException(
                        nameof(precision), 
                        $"precision can't be null for {nativeType}"),
                    scale);
            case MssTypes.Float:
                return new SqlServerFloat(id, name, isNullable);
            case MssTypes.Image:
                return new SqlServerImage(id, name, isNullable);
            case MssTypes.Int:
                return new SqlServerInt(id, name, isNullable);
            case MssTypes.Money:
                return new SqlServerMoney(id, name, isNullable);
            case MssTypes.NChar:
                return new SqlServerNChar(id, name, isNullable, length, collation);
            case MssTypes.NText:
                return new SqlServerNText(id, name, isNullable, collation);
            case MssTypes.NVarChar:
                return new SqlServerNVarChar(id, name, isNullable, length, collation);
            case MssTypes.Real:
                return new SqlServerReal(id, name, isNullable);
            case MssTypes.SmallDateTime:
                return new SqlServerSmallDateTime(id, name, isNullable);
            case MssTypes.SmallInt:
                return new SqlServerSmallInt(id, name, isNullable);
            case MssTypes.SmallMoney:
                return new SqlServerSmallMoney(id, name, isNullable);
            case MssTypes.Text:
                return new SqlServerText(id, name, isNullable, collation);
            case MssTypes.Time:
                return new SqlServerTime(id, name, isNullable, scale);
            case MssTypes.TinyInt:
                return new SqlServerTinyInt(id, name, isNullable);
            case MssTypes.UniqueIdentifier:
                return new SqlServerUniqueIdentifier(id, name, isNullable);
            case MssTypes.VarBinary:
                return new SqlServerVarBinary(id, name, isNullable, length);
            case MssTypes.VarChar:
                return new SqlServerVarChar(id, name, isNullable, length, collation);
            case MssTypes.Xml:
                return new SqlServerXml(id, name, isNullable);
            default:
                throw new ArgumentOutOfRangeException(nameof(nativeType), nativeType, "Unknown native type");
        }
    }
}