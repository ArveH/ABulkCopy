namespace Common.Test
{
    public static class MssTestData
    {
        public static TableDefinition GetTableDefinitionAllTypes()
        {
            return new TableDefinition
            {
                Header = new TableHeader
                {
                    Id = 1,
                    Identity = new Identity(),
                    Location = "default",
                    Name = "MssAllTypes",
                    Schema = "dbo"
                },
                Columns = new List<ColumnDefinition>
                {
                    GetLengthColDefinition(101, "Id", "bigint", 8, 19),
                    GetLengthColDefinition(102, "ExactNumBigInt", "bigint", 8, 19),
                    GetLengthColDefinition(103, "ExactNumInt", "int", 4, 10),
                    GetLengthColDefinition(104, "ExactNumSmallInt", "smallint", 2, 5),
                    GetLengthColDefinition(105, "ExactNumSmallInt", "tinyint", 1, 3),
                    GetLengthColDefinition(106, "ExactNumBit", "bit", 1, 1),
                    GetLengthColDefinition(107, "ExactNumMoney", "money", 8, 19, 4),
                    GetLengthColDefinition(108, "ExactNumSmallMoney", "smallmoney", 4, 10, 4),
                    GetLengthColDefinition(109, "ExactNumDecimal", "decimal", 13, 28, 3),
                    GetLengthColDefinition(110, "ExactNumNumeric", "numeric", 13, 28, 3),
                    GetLengthColDefinition(111, "ApproxNumFloat", "float", 8, 53),
                    GetLengthColDefinition(112, "ApproxNumReal", "real", 4, 24),
                    GetLengthColDefinition(113, "ApproxNumReal", "real", 4, 24),
                    GetLengthColDefinition(114, "DTDate", "date", 3, 10),
                    GetLengthColDefinition(115, "DTDateTime", "datetime", 8, 23, 3),
                    GetLengthColDefinition(116, "DTDateTime2", "datetime2", 8, 27, 7),
                    GetLengthColDefinition(117, "DTSmallDateTime", "smalldatetime", 4, 16),
                    GetLengthColDefinition(118, "DTDateTimeOffset", "datetimeoffset", 10, 34, 7),
                    GetLengthColDefinition(119, "DTTime", "time", 5, 16, 7),
                    GetCharColDefinition(120, "CharStrChar20", "char", 20),
                    GetCharColDefinition(121, "CharStrVarchar20", "varchar", 20),
                    GetCharColDefinition(122, "CharStrVarchar10K", "varchar", -1),
                    GetCharColDefinition(123, "CharStrNChar20", "nchar", 20),
                    GetCharColDefinition(124, "CharStrNVarchar20", "nvarchar", 20),
                    GetCharColDefinition(125, "CharStrNVarchar10K", "nvarchar", -1),
                    GetOtherColDefinition(126, "BinBinary5K", "binary", 5000, true),
                    GetOtherColDefinition(127, "BinVarbinary10K", "varbinary", -1, true),
                    GetOtherColDefinition(128, "OtherGuid", "uniqueidentifier", 16, false),
                    GetOtherColDefinition(129, "OtherXml", "xml", -1, true)
                }
            };
        }

        public static ColumnDefinition GetLengthColDefinition(
            int id,
            string name,
            string dataType,
            int length,
            int precision=0,
            int scale=0)
        {
            var colDef = GetColDefinition(id, name, dataType);
            colDef.Length = length;
            colDef.Precision = precision;
            colDef.Scale = scale;
            return colDef;
        }

        public static ColumnDefinition GetCharColDefinition(
            int id,
            string name,
            string dataType,
            int length)
        {
            var colDef = GetColDefinition(id, name, dataType);
            colDef.Length = length;
            colDef.Precision = 0;
            colDef.Scale = 0;
            colDef.IsNullable = true;
            colDef.Collation = "SQL_Latin1_General_CP1_CI_AS";
            return colDef;
        }

        public static ColumnDefinition GetOtherColDefinition(
            int id,
            string name,
            string dataType,
            int length,
            bool isNullable)
        {
            var colDef = GetColDefinition(id, name, dataType);
            colDef.Length = length;
            colDef.Precision = 0;
            colDef.Scale = 0;
            colDef.IsNullable = isNullable;
            return colDef;
        }

        public static ColumnDefinition GetColDefinition(
            int id,
            string name,
            string dataType)
        {
            return new ColumnDefinition
            {
                Id = id,
                Name = name,
                DataType = dataType,
                Collation = null,
                IsNullable = false,
                Identity = new Identity(),
                DefaultConstraint = null,
                ComputedDefinition = null
            };
        }
    }
}
