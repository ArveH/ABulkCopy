namespace ABulkCopy.Common.TestData;

public static class AllTypes
{
    public static class SampleValues
    {
        public const long BigInt = 123456789012345;
        public const int Int = 123456789;
        public const int SmallInt = 123456;
        public const int TinyInt = 123;
        public const bool Bool = true;
        public const decimal Money = 1234567.12345m;
        public const decimal SmallMoney = 123.123m;
        public const decimal Decimal = 1234567.12345m;
        public const decimal Numeric = 1234567.12345m;
        public const double Float = 1234567.12345d;
        public const double Real = 123.123f;
        public static DateTime Date = new(2023, 03, 31);
        public static DateTime DateTime = new(2023, 03, 31, 11, 12, 13);
        public static DateTime DateTime2 = new(2023, 03, 31, 11, 12, 13);
        public static DateTime SmallDateTime = new(2023, 03, 31, 11, 12, 13);
        public static DateTimeOffset DateTimeOffset = new(new DateTime(2023, 03, 31, 11, 12, 13), new TimeSpan(1, 0, 0));
        public static TimeSpan Time = new(1, 2, 3, 4);
        public const string Char20 = "1234567890";
        public const string Varchar20 = "12345678901234567890";
        public static string Varchar10K = new('a', 10000);
        public const string NChar20 = "ﯵ1234567890";
        public const string NVarchar20 = "123456789ﯵ1234567890";
        public static string NVarchar10K = new('ﯵ', 10000);
        public static byte[] Binary5K = GetBytes(5000);
        public static byte[] Varbinary10K = GetBytes(10000);
        public static Guid Guid = new("A17542D9-A61C-4E4C-8512-DAFFC1416142");
        public const string Xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> <SomeTag>A value</SomeTag>";
    }

    private static byte[] GetBytes(int size)
    {
        var bytes = new byte[size];
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(i % 256);
        }
        return bytes;
    }
}