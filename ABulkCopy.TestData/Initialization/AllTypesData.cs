using SampleValues = ABulkCopy.Common.TestData.AllTypes.SampleValues;

namespace ABulkCopy.TestData.Initialization;

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
            ExactNumTinyInt = SampleValues.TinyInt,
            ExactNumBit = SampleValues.Bool,
            ExactNumMoney = SampleValues.Money,
            ExactNumSmallMoney = SampleValues.SmallMoney,
            ExactNumDecimal = SampleValues.Decimal,
            ExactNumNumeric = SampleValues.Numeric,
            ApproxNumFloat = SampleValues.Float,
            ApproxNumReal = SampleValues.Real,
            DTDate = SampleValues.Date,
            DTDateTime = SampleValues.DateTime,
            DTDateTime2 = SampleValues.DateTime2,
            DTSmallDateTime = SampleValues.SmallDateTime,
            DTDateTimeOffset = SampleValues.DateTimeOffset,
            DTTime = SampleValues.Time,
            CharStrChar20 = SampleValues.Char20,
            CharStrVarchar20 = SampleValues.Varchar20,
            CharStrVarchar10K = SampleValues.Varchar10K,
            CharStrNChar20 = SampleValues.NChar20,
            CharStrNVarchar20 = SampleValues.NVarchar20,
            CharStrNVarchar10K = SampleValues.NVarchar10K,
            BinBinary5K = SampleValues.Binary5K,
            BinVarbinary10K = SampleValues.Varbinary10K,
            OtherGuid = SampleValues.Guid,
            OtherXml = SampleValues.Xml
        };
    }
}