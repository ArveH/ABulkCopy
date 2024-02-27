using CreatePgTestDatabase.Entities;
using SampleValues = ABulkCopy.Common.TestData.AllTypes.SampleValues;

namespace CreatePgTestDatabase.Initialization;

public static class AllTypesData
{
    public static AllTypes Copy()
    {
        return new AllTypes()
        {
            Id = 1,
            ExactNumBigInt = SampleValues.BigInt,
            ExactNumInt = SampleValues.Int,
            ExactNumSmallInt = SampleValues.SmallInt,
            ExactNumBool = SampleValues.Bool,
            ExactNumMoney = SampleValues.Money,
            ExactNumDecimal = SampleValues.Decimal,
            ExactNumNumeric = SampleValues.Numeric,
            ApproxNumFloat = SampleValues.Float,
            ApproxNumReal = SampleValues.Real,
            DTDate = SampleValues.Date,
            DTTimeStamp = SampleValues.DateTime,
            DTTimeStampTz= SampleValues.DateTimeOffset,
            DTTime = SampleValues.Time,
            CharStrChar20 = SampleValues.Char20,
            CharStrVarchar20 = SampleValues.Varchar20,
            CharStrVarchar10K = SampleValues.Varchar10K,
            BinByteA5K = SampleValues.Binary5K,
            BinByteA10K = SampleValues.Varbinary10K,
            OtherUuid = SampleValues.Guid
        };
    }
}