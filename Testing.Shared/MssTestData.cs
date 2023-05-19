namespace Testing.Shared;

public static class MssTestData
{
    public static TableDefinition GetEmpty(string name)
    {
        return new TableDefinition
        {
            Header = new TableHeader
            {
                Id = 1,
                Location = "default",
                Name = name,
                Schema = "dbo"
            }
        };
    }

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
            Columns = new List<IColumn>
            {

                GetIdColDefinition(101, "Id"),
                new SqlServerBigInt(102, "ExactNumBigInt", false),
                new SqlServerInt(103, "ExactNumInt", false),
                new SqlServerSmallInt(104, "ExactNumSmallInt", false),
                new SqlServerTinyInt(105, "ExactNumSmallInt", false),
                new SqlServerBit(106, "ExactNumBit", false),
                new SqlServerMoney(107, "ExactNumMoney", false),
                new SqlServerSmallMoney(108, "ExactNumSmallMoney", false),
                new SqlServerDecimal(109, "ExactNumDecimal", false, 28, 3),
                new SqlServerFloat(111, "ApproxNumFloat", false),
                new SqlServerReal(112, "ApproxNumReal", false),
                new SqlServerDate(114, "DTDate", false),
                new SqlServerDateTime(115, "DTDateTime", false),
                new SqlServerDateTime2(116, "DTDateTime2", false),
                new SqlServerSmallDatetime(117, "DTSmallDateTime", false),
                new SqlServerDatetimeOffset(118, "DTDateTimeOffset", false, 7),
                new SqlServerTime(119, "DTTime", false, 7),
                new SqlServerChar(120, "CharStrChar20", false, 20),
                new SqlServerVarChar(121, "CharStrVarchar20", false, 20),
                new SqlServerVarChar(122, "CharStrVarchar10K", false, -1),
                new SqlServerNChar(123, "CharStrNChar20", false, 20),
                new SqlServerNVarChar(124, "CharStrNVarchar20", false, 20),
                new SqlServerNVarChar(125, "CharStrNVarchar10K", false, -1),
                new SqlServerBinary(126, "BinBinary5K", true, 5000),
                new SqlServerVarBinary(127, "BinVarbinary10K", true, -1),
                new SqlServerUniqueIdentifier(128, "OtherGuid", false)
            }
        };
    }

    public static TableDefinition GetTableDefaults()
    {
        return new TableDefinition
        {
            Header = new TableHeader
            {
                Id = 1,
                Identity = new Identity(),
                Location = "default",
                Name = "MssDefaults",
                Schema = "dbo"
            },
            Columns = new List<IColumn>
            {
                GetIdColDefinition(101, "Id"),
                new SqlServerInt(101, "intdef", false)
                {
                    DefaultConstraint = new DefaultDefinition
                    {
                        Name = "df_bulkcopy_int",
                        Definition = "((0))",
                        IsSystemNamed = false
                    }
                },
                new SqlServerNVarChar(102, "strdef", true, 20, "SQL_Latin1_General_CP1_CI_AS")
                {
                    DefaultConstraint = new DefaultDefinition
                    {
                        Name = "DF__arveh__col1__531856C7",
                        Definition = "('Norway')",
                        IsSystemNamed = true
                    }
                },
                new SqlServerDateTime2(103, "datedef", true)
                {
                    DefaultConstraint = new DefaultDefinition
                    {
                        Name = "DF__arveh__col1__531856C8",
                        Definition = "(getdate())",
                        IsSystemNamed = true
                    }
                }
            }
        };
    }

    public static IColumn GetIdColDefinition(int id, string name)
    {
        return new SqlServerBigInt(id, name, false)
        {
            Identity = new Identity()
        };
    }
}